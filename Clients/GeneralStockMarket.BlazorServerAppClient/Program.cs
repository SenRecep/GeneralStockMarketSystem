
using System;
using System.Threading.Tasks;

using GeneralStockMarket.CoreLib.ExtensionMethods;

using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

using Serilog;

namespace GeneralStockMarket.BlazorServerAppClient
{
    public class Program
    {
        public static async Task<int> Main(string[] args)
        {
            try
            {
                IHost host = CreateHostBuilder(args).Build();

                Log.Information("Starting host...");
                await host.RunAsync();
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
