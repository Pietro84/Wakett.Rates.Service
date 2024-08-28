using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text.Json;
using System.Threading.Tasks;
using Wakett.Rates.Service.Core.Interfaces;
using Wakett.Rates.Service.Core.Models;
using JsonException = System.Text.Json.JsonException;
using JsonSerializer = System.Text.Json.JsonSerializer;

namespace Wakett.Rates.Service.Core.Services
{
    public class ApiService : IApiService
    {
        private readonly HttpClient _httpClient;
        private static string API_KEY = "d04b7372-e247-4fb4-807c-c0e4e61ef81b";

        public ApiService(HttpClient httpClient)
        {
            _httpClient = httpClient;
            _httpClient.DefaultRequestHeaders.Add("X-CMC_PRO_API_KEY", API_KEY);
            _httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        }

        public async Task<Dictionary<string, decimal>> GetLatestCryptoCurrencyAsync()
        {
            try
            {
                var response = await _httpClient.GetAsync("https://pro-api.coinmarketcap.com/v1/cryptocurrency/listings/latest");
                CryptocurrencyData cryptoData = null;

                var latestRates = new Dictionary<string, decimal>();

                if (response.IsSuccessStatusCode)
                {
                    var content = await response.Content.ReadAsStringAsync();
                    try
                    {
                        using (var contentStream = new MemoryStream(System.Text.Encoding.UTF8.GetBytes(content)))
                        {
                            cryptoData = await JsonSerializer.DeserializeAsync<CryptocurrencyData>(contentStream, new JsonSerializerOptions
                            {
                                PropertyNameCaseInsensitive = true
                            });
                        }
                    }
                    catch (JsonException jsonEx)
                    {
                        //TODO LOG
                        //Console.WriteLine($"Errore di deserializzazione: {jsonEx.Message}");
                    }

                    if (cryptoData?.Data != null)
                    {
                        latestRates = cryptoData.Data.ToDictionary(
                               c => c.Symbol+"/"+"USD",
                               c => c.Quote["USD"].Price
                           );
                        //foreach (var crypto in cryptoData.Data)
                        //{
                        //    if (crypto.Quote.ContainsKey("USD"))
                        //    {
                        //        prices[crypto.Symbol] = crypto.Quote["USD"].Price;
                        //    }
                        //}
                    }

                    return latestRates;
                }

                //response.EnsureSuccessStatusCode(); // Lancia un'eccezione se la risposta non è di successo
                //return await response.Content.ReadAsStringAsync();
            }
            catch (HttpRequestException ex)
            {
                // Gestisci eventuali errori legati alla richiesta HTTP

            }
            return null;
        }
    }
}
