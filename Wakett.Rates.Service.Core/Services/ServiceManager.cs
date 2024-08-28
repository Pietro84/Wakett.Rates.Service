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
        private readonly IConfigurationService _configurationService;
        private readonly ICryptocurrencyService _cryptocurrencyService;
        private readonly ILogService _logService;
        private readonly IApiService _apiService;

        private readonly IBus _bus;

        public ServiceManager(IConfigurationService configurationService, 
                              ILogService logService, IApiService apiService, ICryptocurrencyService cryptocurrencyService, IBus bus)
        {
            _configurationService = configurationService;
            _cryptocurrencyService = cryptocurrencyService;
            _logService = logService;
            _apiService = apiService;
            _bus = bus;
        }

        public void ExecuteTasks()
        {
            //Get Task Configuration from Scheduler
            var taskConfig = _configurationService.GetTaskConfiguration();

            if (taskConfig == null)
            {
                _logService.Log("No task configuration found.", "Error");
                return;
            }

            if (ShouldExecuteTask(taskConfig.LastRunTime, taskConfig.TickTime))
            {
                try
                {
                    _logService.Log("Executing task...", "Info");
                    ExecuteTask().Wait();
                    _configurationService.UpdateLastRunTime(taskConfig.TaskId, DateTime.Now);
                    _logService.Log("Task executed successfully.", "Info");
                }
                catch (Exception ex)
                {
                    _logService.Log($"Error executing task: {ex.Message}", "Error");
                }
            }
        }

        private async Task ExecuteTask()
        {
            var prices = await _apiService.GetLatestCryptoCurrencyAsync();
            if (prices != null && prices.Count > 0)
            {
                //aggiorno quote
                await _cryptocurrencyService.UpsertCryptocurrencyQuotesAsync(prices);

                //recupero solo le nuove quote (in stato NEW)
                var newQuotes = await _cryptocurrencyService.GetNewQuotesAsync();

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
