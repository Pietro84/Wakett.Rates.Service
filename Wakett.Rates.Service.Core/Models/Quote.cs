using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Wakett.Rates.Service.Core.Models
{
    public class Quote
    {
        [JsonPropertyName("price")]
        public decimal Price { get; set; }

        [JsonPropertyName("volume_24h")]
        public decimal Volume24h { get; set; }

        [JsonPropertyName("volume_change_24h")]
        public decimal VolumeChange24h { get; set; }

        [JsonPropertyName("percent_change_1h")]
        public decimal PercentChange1h { get; set; }

        [JsonPropertyName("percent_change_24h")]
        public decimal PercentChange24h { get; set; }

        [JsonPropertyName("percent_change_7d")]
        public decimal PercentChange7d { get; set; }

        [JsonPropertyName("percent_change_30d")]
        public decimal PercentChange30d { get; set; }

        [JsonPropertyName("percent_change_60d")]
        public decimal PercentChange60d { get; set; }

        [JsonPropertyName("percent_change_90d")]
        public decimal PercentChange90d { get; set; }

        [JsonPropertyName("market_cap")]
        public decimal MarketCap { get; set; }

        [JsonPropertyName("market_cap_dominance")]
        public decimal MarketCapDominance { get; set; }

        [JsonPropertyName("fully_diluted_market_cap")]
        public decimal FullyDilutedMarketCap { get; set; }

        [JsonPropertyName("tvl")]
        public object Tvl { get; set; }

        [JsonPropertyName("last_updated")]
        public string LastUpdated { get; set; }
    }
}
