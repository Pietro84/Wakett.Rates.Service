using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Wakett.Rates.Service.Core.Models
{
    public class CryptocurrencyRatesUpdated
    {
        public string Symbol { get; set; }
        public decimal NewPrice { get; set; }
        //public DateTime LastUpdated { get; set; }
    }
}
