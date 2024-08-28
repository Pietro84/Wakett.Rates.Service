using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using Rebus.Activation;
using Rebus.Bus;
using Rebus.Config;
using Rebus.Routing.TypeBased;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using System.Threading.Tasks;
using Wakett.Rates.Service.Core.Interfaces;
using Wakett.Rates.Service.Core.Models;
using Wakett.Rates.Service.Core.Services;
using Wakett.Rates.Service.Infrastructure.Repositories;

namespace Wakett.Rates.Service
{
    class Program
    {
        static void Main()
        {
            var services = new ServiceCollection();
            ConfigureServices(services);
            var serviceProvider = services.BuildServiceProvider();
            var service = serviceProvider.GetService<Service1>();
            ServiceBase.Run(service);
        }

        private static void ConfigureServices(IServiceCollection services)
        {
            string connectionString = ConfigurationManager.ConnectionStrings["DatabaseConnectionString"].ConnectionString;
            //services.AddRebus(configure => configure.Transport(t => t.UseSqlServer(connectionStringRebus, "RatesQueue")).Routing(r => r.TypeBased().Map<CryptocurrencyQuoteUpdated>("EventQueue")));
            BusConfiguration(services);
            services.AddSingleton<ICryptocurrencyRepository>(new CryptocurrencyRepository(connectionString));
            services.AddSingleton<IConfigurationRepository>(new ConfigurationRepository(connectionString));
            services.AddSingleton<ILogRepository>(new LogRepository(connectionString));
            services.AddSingleton<ICryptocurrencyService, CryptocurrencyService>();
            services.AddSingleton<IConfigurationService, ConfigurationService>();
            services.AddSingleton<ILogService, LogService>();
            services.AddSingleton<IServiceManager, ServiceManager>();
            services.AddHttpClient<IApiService, ApiService>();
            services.AddSingleton<Service1>();
        }

        private static void BusConfiguration(IServiceCollection services)
        {
            string connectionStringRebus = ConfigurationManager.ConnectionStrings["RebusConnectionString"].ConnectionString;
            // Configurazione di Rebus nel contesto del Dependency Injection
            services.AddRebus(configure => configure
                    .Transport(t => t.UseSqlServer(new SqlServerLeaseTransportOptions(connectionStringRebus), "RatesUpdatedQueue"))
                    .Subscriptions(s => s.StoreInSqlServer(connectionStringRebus, "Subscriptions", true))
                    .Routing(r => r.TypeBased().Map<CryptocurrencyRatesUpdated>("RatesUpdatedQueue"))
                    .Options(o => o.SetBusName("default")),true);
        }
    }
}
