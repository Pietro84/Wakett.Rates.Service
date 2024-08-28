using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Wakett.Rates.Service.Core.Interfaces;

namespace Wakett.Rates.Service.Core.Services
{
    public class LogService : ILogService
    {
        private readonly ILogRepository _logRepository;
        public LogService(ILogRepository logRepository) 
        { 
            _logRepository = logRepository;
        }
        public void Log(string message, string type)
        {
            _logRepository.Log(message, type);
        }
    }
}
