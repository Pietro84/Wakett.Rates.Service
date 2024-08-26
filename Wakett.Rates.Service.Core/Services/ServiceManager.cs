using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Wakett.Rates.Service.Core.Interfaces;

namespace Wakett.Rates.Service.Core.Services
{
    public class ServiceManager : IServiceManager
    {
        private readonly IConfigurationRepository _configurationRepository;
        private readonly ILogRepository _logRepository;
        private readonly IApiService _apiService;
        private readonly ICryptocurrencyRepository _cryptocurrencyRepository;

        public ServiceManager(IConfigurationRepository configurationRepository, ILogRepository logRepository, IApiService apiService, ICryptocurrencyRepository cryptocurrencyRepository)
        {
            _configurationRepository = configurationRepository;
            _logRepository = logRepository;
            _apiService = apiService;
            _cryptocurrencyRepository = cryptocurrencyRepository;
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
                await _cryptocurrencyRepository.UpsertCryptocurrencyQuotesAsync(prices);
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
