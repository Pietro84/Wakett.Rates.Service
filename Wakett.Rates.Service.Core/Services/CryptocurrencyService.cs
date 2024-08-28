using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Wakett.Rates.Service.Core.Interfaces;
using Wakett.Rates.Service.Core.Models;

namespace Wakett.Rates.Service.Core.Services
{
    public class CryptocurrencyService : ICryptocurrencyService
    {
        private readonly ICryptocurrencyRepository _cryptocurrencyRepository;

        public CryptocurrencyService(ICryptocurrencyRepository cryptocurrencyRepository) 
        { 
            _cryptocurrencyRepository = cryptocurrencyRepository;
        }

        public async Task UpsertCryptocurrencyQuotesAsync(Dictionary<string, decimal> prices)
        {
            await _cryptocurrencyRepository.UpsertCryptocurrencyQuotesAsync(prices);   
        }

        public async Task<IEnumerable<CryptocurrencyQuoteUpdated>> GetNewQuotesAsync()
        {
            return await _cryptocurrencyRepository.GetNewQuotesAsync();
        }
    }
}
