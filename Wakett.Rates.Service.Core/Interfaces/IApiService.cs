using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Wakett.Rates.Service.Core.Interfaces
{
    public interface IApiService
    {
        Task<Dictionary<string, decimal>> GetLatestCryptoCurrencyAsync();
    }
}
