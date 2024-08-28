using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Wakett.Rates.Service.Core.Interfaces
{
    public interface ILogService
    {
        void Log(string message, string type);
    }
}
