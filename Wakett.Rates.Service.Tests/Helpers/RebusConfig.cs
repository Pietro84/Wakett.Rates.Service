using Microsoft.Extensions.DependencyInjection;
using Rebus.Bus;
using Rebus.Config;
using Rebus.Routing.TypeBased;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Wakett.Rates.Service.Core.Models;
using Rebus.Serialization.Json;

namespace Wakett.Rates.Service.Tests.Helpers
{

    public class RebusConfig
    {
        public static IBus ConfigureBus()
        {

            // Configura i servizi
            var services = new ServiceCollection();
            string connectionStringRebus = "Server=DESKTOP-1330PDH\\SEQSQL100;Database=Rebus;User Id=dbo_Wakett;Password=C66CCB6C-3580-46A7-A2B9-C0C16504BB98;integrated security=false;MultipleActiveResultSets=true";
            services.AddRebus(configure => configure.Transport(t => t.UseSqlServer(connectionStringRebus, "RatesQueue")).Routing(r => r.TypeBased().Map<CryptocurrencyQuoteUpdated>("EventQueue")));

            // Costruisci lo service provider
            var serviceProvider = services.BuildServiceProvider();

            // Avvia Rebus
            var bus = serviceProvider.GetRequiredService<IBus>();
            return bus;


        }
    }
}
