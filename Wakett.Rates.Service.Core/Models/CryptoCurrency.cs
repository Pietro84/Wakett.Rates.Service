using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Wakett.Rates.Service.Core.Models
{
    public class Cryptocurrency
    {
        [JsonPropertyName("id")]
        public int Id { get; set; }

        [JsonPropertyName("name")]
        public string Name { get; set; }

        [JsonPropertyName("symbol")]
        public string Symbol { get; set; }

        [JsonPropertyName("slug")]
        public string Slug { get; set; }

        [JsonPropertyName("num_market_pairs")]
        public int NumMarketPairs { get; set; }

        [JsonPropertyName("date_added")]
        public string DateAdded { get; set; }

        [JsonPropertyName("tags")]
        public List<string> Tags { get; set; }

        [JsonPropertyName("max_supply")]
        public decimal? MaxSupply { get; set; }

        [JsonPropertyName("circulating_supply")]
        public decimal CirculatingSupply { get; set; }

        [JsonPropertyName("total_supply")]
        public decimal TotalSupply { get; set; }

        [JsonPropertyName("infinite_supply")]
        public bool InfiniteSupply { get; set; }

        [JsonPropertyName("platform")]
        public object Platform { get; set; }

        [JsonPropertyName("cmc_rank")]
        public int CmcRank { get; set; }

        [JsonPropertyName("self_reported_circulating_supply")]
        public object SelfReportedCirculatingSupply { get; set; }

        [JsonPropertyName("self_reported_market_cap")]
        public object SelfReportedMarketCap { get; set; }

        [JsonPropertyName("tvl_ratio")]
        public object TvlRatio { get; set; }

        [JsonPropertyName("last_updated")]
        public string LastUpdated { get; set; }

        [JsonPropertyName("quote")]
        public Dictionary<string, Quote> Quote { get; set; }
    }
}
