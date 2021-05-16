// Copyright (c) Brock Allen & Dominick Baier. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.


using System;
using System.Collections.Generic;
using System.Threading.Tasks;

using GeneralStockMarket.AuthAPI.Data;
using GeneralStockMarket.AuthAPI.Seeding;
using GeneralStockMarket.CoreLib.ExtensionMethods;

using IdentityServer4.EntityFramework.DbContexts;

using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

using Serilog;

namespace GeneralStockMarket.AuthAPI
{
    public class Program
    {
        public static async Task<int> Main(string[] args)
        {
            try
            {
                IHost host = CreateHostBuilder(args).Build();

                using IServiceScope serviceScope = host.Services.CreateScope();

                IServiceProvider services = serviceScope.ServiceProvider;

                ConfigurationDbContext configurationDbContext = services.GetRequiredService<ConfigurationDbContext>();
                ApplicationDbContext applicationDbContext = services.GetRequiredService<ApplicationDbContext>();
                PersistedGrantDbContext  persistedGrantDbContext = services.GetRequiredService<PersistedGrantDbContext>();

                await applicationDbContext.Database.MigrateAsync();
                await configurationDbContext.Database.MigrateAsync();
                await persistedGrantDbContext.Database.MigrateAsync();

                List<Task> tasks = new List<Task>
                {
                    IdentityServerSeedData.SeedConfiguration(configurationDbContext),
                    IdentityServerSeedData.SeedUserData(services)
                };

                await Task.WhenAll(tasks);

                Log.Information("Starting host...");
                host.Run();
                return 0;
            }
            catch (Exception ex)
            {
                Log.Fatal(ex, "Host terminated unexpectedly.");
                return 1;
            }
            finally
            {
                Log.CloseAndFlush();
            }
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
             Host.CreateDefaultBuilder(args)
                    .ConfigureWebHostDefaults(webBuilder =>
                    {
                        webBuilder.UseStartup<Startup>()
                        .ConfigureLogging((hostingContext, config) =>
                        {
                            config.ClearProviders();
                            config.AddSerilog(LoggerExtensionMethods.SerilogInit());
                        });
                    }).ConfigureAppConfiguration((context, config) =>
                    {
                        config.AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);
                    });
    }
}