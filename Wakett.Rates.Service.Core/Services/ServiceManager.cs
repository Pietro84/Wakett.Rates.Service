using Rebus.Bus;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Wakett.Rates.Service.Core.Interfaces;
using Wakett.Rates.Service.Core.Models;

namespace Wakett.Rates.Service.Core.Services
{
    public class ServiceManager : IServiceManager
    {
        private readonly IConfigurationRepository _configurationRepository;
        private readonly ILogRepository _logRepository;
        private readonly IApiService _apiService;
        private readonly ICryptocurrencyRepository _cryptocurrencyRepository;
        private readonly IBus _bus;

        public ServiceManager(IConfigurationRepository configurationRepository, ILogRepository logRepository, IApiService apiService, ICryptocurrencyRepository cryptocurrencyRepository, IBus bus)
        {
            _configurationRepository = configurationRepository;
            _logRepository = logRepository;
            _apiService = apiService;
            _cryptocurrencyRepository = cryptocurrencyRepository;
            _bus = bus;
        }

        public void ExecuteTasks()
        {
            //Get Task Configuration from Scheduler
            var taskConfig = _configurationRepository.GetTaskConfiguration();

            if (taskConfig == null)
            {
                _logRepository.Log(0, "No task configuration found.", "Error");
                return;
            }

            if (ShouldExecuteTask(taskConfig.LastRunTime, taskConfig.TickTime))
            {
                try
                {
                    _logRepository.Log(taskConfig.TaskId, "Executing task...", "Info");
                    ExecuteTask().Wait();
                    _configurationRepository.UpdateLastRunTime(taskConfig.TaskId, DateTime.Now);
                    _logRepository.Log(taskConfig.TaskId, "Task executed successfully.", "Info");
                }
                catch (Exception ex)
                {
                    _logRepository.Log(taskConfig.TaskId, $"Error executing task: {ex.Message}", "Error");
                }
            }
        }

        private async Task ExecuteTask()
        {
            var prices = await _apiService.GetLatestCryptoCurrencyAsync();
            if (prices != null && prices.Count > 0)
            {
                //aggiorno quote
                await _cryptocurrencyRepository.UpsertCryptocurrencyQuotesAsync(prices);

                //recupero solo le nuove quote (in stato NEW)
                var newQuotes = await _cryptocurrencyRepository.GetNewQuotesAsync();

                foreach (var quote in newQuotes)
                {
                    var message = new CryptocurrencyQuoteUpdated
                    {
                        Symbol = quote.Symbol,
                        NewPrice = quote.NewPrice
                    };

                    //invio messaggio
                    await _bus.Publish(message);
                }

            }
        }

        private bool ShouldExecuteTask(DateTime? lastRunTime, int tickTime)
        {
            DateTime now = DateTime.Now;
            if (!lastRunTime.HasValue) return true;

            return now >= lastRunTime.Value.AddMinutes(tickTime);
        }
    }
}
