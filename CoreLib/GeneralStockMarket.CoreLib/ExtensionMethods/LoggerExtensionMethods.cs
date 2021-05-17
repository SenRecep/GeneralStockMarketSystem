using System;

using GeneralStockMarket.CoreLib.Response;

using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;

using Serilog;
using Serilog.Events;
using Serilog.Sinks.SystemConsole.Themes;

namespace GeneralStockMarket.CoreLib.ExtensionMethods
{
    public static class LoggerExtensionMethods
    {
        public static Serilog.ILogger SerilogInit()
        {
            string logTemplate = "[{Timestamp:HH:mm:ss} {Level}] {SourceContext}{NewLine}{Message:lj}{NewLine}{Exception}{NewLine}";

            Log.Logger = new LoggerConfiguration()
                .Enrich.FromLogContext()
                .MinimumLevel.Debug()
                .MinimumLevel.Override("Microsoft", LogEventLevel.Warning)
                .MinimumLevel.Override("Microsoft.Hosting.Lifetime", LogEventLevel.Information)
                .MinimumLevel.Override("System", LogEventLevel.Warning)
                .MinimumLevel.Override("Microsoft.AspNetCore.Authentication", LogEventLevel.Information)
                .WriteTo.Debug(outputTemplate: DateTime.Now.ToString())
                //.WriteTo.Console(new CompactJsonFormatter())
                .WriteTo.Console(outputTemplate: logTemplate, theme: AnsiConsoleTheme.Literate)
                .WriteTo.File("Logs/log.txt", rollingInterval: RollingInterval.Day, outputTemplate: logTemplate)
                .CreateLogger();
            return Log.Logger;
        }

        public static void LogResponse<T, R>(this ILogger<T> logger, Response<R> response, string message = "") where R : class
        {
            if (!message.IsEmpty())
                logger.LogInformation(message);

            if (response.IsSuccessful && response.Data is string res)
                logger.LogInformation(res);
           
            if (!response.IsSuccessful)
            {
                string errors = Error.GetError(response.ErrorData);
                if (response.StatusCode == StatusCodes.Status500InternalServerError)
                    logger.LogError(errors);
                else
                    logger.LogWarning(errors);
            }
        }
    }
}
