using Rebus.Handlers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Wakett.Rates.Service.Core.Models;

namespace Wakett.Rates.Service.Tests.Helpers
{
    public class HubHandler : IHandleMessages<CryptocurrencyRatesUpdated>
    {
        public HubHandler() { }


        public async Task Handle(CryptocurrencyRatesUpdated message)
        {
            //...
        }
    }
}
