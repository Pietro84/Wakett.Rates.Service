using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Wakett.Rates.Service.Core.Interfaces;
using Wakett.Rates.Service.Core.Services;
using Wakett.Rates.Service.Infrastructure.Repositories;
using Xunit;

namespace Wakett.Rates.Service.Tests.Services
{
    public class ApiServiceIntegrationTests
    {
        private readonly ApiService _apiClient;
        private readonly ICryptocurrencyRepository _cryptocurrencyRepository;
        private readonly string _connectionString = "Server=DESKTOP-1330PDH\\SEQSQL100;Database=Wakett;User Id=dbo_Wakett;Password=C66CCB6C-3580-46A7-A2B9-C0C16504BB98;integrated security=false;MultipleActiveResultSets=true";

        public ApiServiceIntegrationTests()
        {
            var httpClient = new HttpClient();
            _apiClient = new ApiService(httpClient);
            _cryptocurrencyRepository = new CryptocurrencyRepository(_connectionString);
        }

        [Fact]
        public async Task GetCryptocurrencyPricesAsync_ReturnsExpectedPrices()
        {
            // Act
            var prices = await _apiClient.GetLatestCryptoCurrencyAsync();
            if (prices != null && prices.Count > 0)
            {
                await _cryptocurrencyRepository.UpsertCryptocurrencyQuotesAsync(prices);
            }
            // Assert
            Assert.NotNull(prices);
            Assert.Contains("BTC", prices.Keys); // Verifica se il dizionario contiene la chiave "BTC"
            Assert.True(prices["BTC"] > 0); // Verifica se il valore per la chiave "BTC" è maggiore di zero
        }
    }
}
