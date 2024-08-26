using Moq;
using Moq.Protected;
using Newtonsoft.Json;
using System;
using System.Net;
using System.Net.Http;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using Wakett.Rates.Service.Core.Interfaces;
using Wakett.Rates.Service.Core.Models;
using Wakett.Rates.Service.Core.Services;
using Wakett.Rates.Service.Tests.Helpers;
using Xunit;

namespace MyScheduledTaskService.Tests.Services
{
    public class ApiServiceTests
    {
        private readonly Mock<HttpMessageHandler> _mockHandler;
        private readonly HttpClient _httpClient;
        private readonly ApiService _apiService;

        public ApiServiceTests()
        {
            _mockHandler = new Mock<HttpMessageHandler>();
            _httpClient = new HttpClient(_mockHandler.Object);
            _apiService = new ApiService(_httpClient);
        }

        [Fact]
        public async Task GetLatestCryptoCurrencyAsync_ShouldReturnSuccess_WhenApiCallSucceeds()
        {
            // Arrange
            _mockHandler.ReturnsResponse(HttpMethod.Get, "https://pro-api.coinmarketcap.com/v1/cryptocurrency/listings/latest", HttpStatusCode.OK, "Success");

            // Act
            var result = await _apiService.GetLatestCryptoCurrencyAsync();

            // Assert
            //Assert.Equal("Success", result);
        }

        [Fact]
        public Task DeserializationTests()
        {
            var jsonResponse = @"
        {
            ""status"": {
                ""timestamp"": ""2024-08-25T16:43:35.441Z"",
                ""error_code"": 0,
                ""error_message"": null,
                ""elapsed"": 24,
                ""credit_count"": 1,
                ""notice"": null,
                ""total_count"": 9991
            },
            ""data"": [
                {
                    ""id"": 1,
                    ""name"": ""Bitcoin"",
                    ""symbol"": ""BTC"",
                    ""slug"": ""bitcoin"",
                    ""num_market_pairs"": 11649,
                    ""date_added"": ""2010-07-13T00:00:00.000Z"",
                    ""tags"": [""mineable""],
                    ""max_supply"": 21000000,
                    ""circulating_supply"": 19744968,
                    ""total_supply"": 19744968,
                    ""infinite_supply"": false,
                    ""platform"": null,
                    ""cmc_rank"": 1,
                    ""self_reported_circulating_supply"": null,
                    ""self_reported_market_cap"": null,
                    ""tvl_ratio"": null,
                    ""last_updated"": ""2024-08-25T16:42:00.000Z"",
                    ""quote"": {
                        ""USD"": {
                            ""price"": 64096.38634459169
                        }
                    }
                }
            ]
        }";

            var cryptoData = System.Text.Json.JsonSerializer.Deserialize<CryptocurrencyData>(jsonResponse, new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            });

            if (cryptoData != null && cryptoData.Data != null)
            {
                Console.WriteLine($"Test Completed");
            }

            return Task.CompletedTask;
        }

        //[Fact]
        //public async Task GetLatestCryptoCurrencyAsync_ShouldReturnError_WhenApiCallFails()
        //{
        //    // Arrange
        //    _mockHandler.ReturnsResponse(HttpMethod.Get, "https://your-minimal-api-endpoint/api/your-endpoint", HttpStatusCode.InternalServerError, "Request failed");

        //    // Act
        //    var result = await _apiService.GetLatestCryptoCurrencyAsync();

        //    // Assert
        //    Assert.Contains("Error calling API", result);
        //}
    }
}
