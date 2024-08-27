﻿using Microsoft.Extensions.DependencyInjection;
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
            string connectionStringRebus = ConfigurationManager.ConnectionStrings["RebusConnectionString"].ConnectionString;
            services.AddRebus(configure => configure.Transport(t => t.UseSqlServer(connectionStringRebus, "RatesQueue")).Routing(r => r.TypeBased().Map<CryptocurrencyQuoteUpdated>("EventQueue")));
            services.AddSingleton<IConfigurationRepository>(new ConfigurationRepository(connectionString));
            services.AddSingleton<ILogRepository>(new LogRepository(connectionString));
            services.AddSingleton<ICryptocurrencyRepository>(new CryptocurrencyRepository(connectionString));
            services.AddSingleton<IServiceManager, ServiceManager>();
            services.AddHttpClient<IApiService, ApiService>();
            services.AddSingleton<Service1>();
        }
    }
}
