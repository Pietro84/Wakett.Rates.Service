using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Wakett.Rates.Service.Core.Models
{
    public class CryptocurrencyData
    {
        [JsonPropertyName("status")]
        public Status Status { get; set; }

        [JsonPropertyName("data")]
        public List<Cryptocurrency> Data { get; set; }
    }
}
