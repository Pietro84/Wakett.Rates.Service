using Rebus.Activation;
using Rebus.Bus;
using Rebus.Config;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using Wakett.Rates.Service.Core.Interfaces;
using Wakett.Rates.Service.Core.Models;
using Wakett.Rates.Service.Core.Services;
using Wakett.Rates.Service.Infrastructure.Repositories;
using Wakett.Rates.Service.Tests.Helpers;
using Xunit;

namespace Wakett.Rates.Service.Tests.Services
{
    public class ServiceIntegrationTests
    {
        private readonly ApiService _apiClient;
        private readonly ICryptocurrencyRepository _cryptocurrencyRepository;
        private readonly string _connectionString = "Server=DESKTOP-1330PDH\\SEQSQL100;Database=Wakett;User Id=dbo_Wakett;Password=C66CCB6C-3580-46A7-A2B9-C0C16504BB98;integrated security=false;MultipleActiveResultSets=true";
       
        public ServiceIntegrationTests()
        {
            var httpClient = new HttpClient();
            _apiClient = new ApiService(httpClient);
            _cryptocurrencyRepository = new CryptocurrencyRepository(_connectionString);
        }

        [Fact]
        public async Task TestBus()
        {
            try
            {
                var message = new CryptocurrencyRatesUpdated
                {
                    Symbol = "ABC/USD",
                    NewPrice = 12345.67m
                };
                var bus = RebusConfig.ConfigureBus();
                await bus.Send(message);
            }
            catch(Exception ex)
            {
                string s = ex.Message;
            }
            
        }

        [Fact]
        public async Task ExecuteAsync()
        {
            //Recupero le quote via API
            var prices = await _apiClient.GetLatestCryptoCurrencyAsync();
            if (prices != null && prices.Count > 0)
            {
                //Aggiorno delle quote scaricate in base alla condizione nella stored
                await _cryptocurrencyRepository.UpsertCryptocurrencyRatesAsync(prices);

                //Recupero solo le nuove quote (quelle in stato NEW)
                var newRates = await _cryptocurrencyRepository.GetNewRatesAsync();
                List<CryptocurrencyRatesUpdated> message = new List<CryptocurrencyRatesUpdated>();
                if(newRates != null)
                {
                    foreach (var quote in newRates)
                    {
                        var item = new CryptocurrencyRatesUpdated
                        {
                            Symbol = quote.Symbol,
                            NewPrice = quote.NewPrice
                        };
                        message.Add(item);
                    }

                    //Invio messaggi delle quote
                    var bus = RebusConfig.ConfigureBus();
                    await bus.Send(message);
                }

            }

            // Assert
            //Assert.NotNull(prices);
            //Assert.Contains("BTC/USD", prices.Keys); //Verifica se il dizionario contiene la chiave "BTC/USD"
            //Assert.True(prices["BTC/USD"] > 0); //Verifica se il valore per la chiave "BTC" è maggiore di zero
        }
    }
}
