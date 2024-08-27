using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Wakett.Rates.Service.Core.Models;

namespace Wakett.Rates.Service.Core.Interfaces
{
    public interface ICryptocurrencyRepository
    {
        Task UpsertCryptocurrencyQuotesAsync(Dictionary<string, decimal> prices);
        Task<IEnumerable<CryptocurrencyQuoteUpdated>> GetNewQuotesAsync();
    }
}
