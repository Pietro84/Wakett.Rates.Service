using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Wakett.Rates.Service.Core.Interfaces;
using Wakett.Rates.Service.Core.Models;

namespace Wakett.Rates.Service.Core.Services
{
    public class ConfigurationService : IConfigurationService
    {
        private readonly IConfigurationRepository _configurationRepository;
        public ConfigurationService(IConfigurationRepository configurationRepository) 
        {
            _configurationRepository = configurationRepository;
        }

        public TaskConfiguration GetTaskConfiguration()
        {
            return _configurationRepository.GetTaskConfiguration();
        }
        public void UpdateLastRunTime(int taskId, DateTime lastRunTime)
        {
            _configurationRepository.UpdateLastRunTime(taskId, lastRunTime);
        }
    }
}
