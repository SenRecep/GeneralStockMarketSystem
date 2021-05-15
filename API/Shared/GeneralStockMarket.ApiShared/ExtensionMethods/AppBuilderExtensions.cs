
using System.Threading.Tasks;

using GeneralStockMarket.ApiShared.Models;
using GeneralStockMarket.CoreLib.Response;

using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Http;

using Newtonsoft.Json;

namespace GeneralStockMarket.ApiShared.ExtensionMethods
{
    public static class AppBuilderExtensions
    {
        public static void UseCustomExceptionHandler(this IApplicationBuilder app)
        {
            app.UseExceptionHandler(config =>
            {
                config.Run(async context =>
                {
                    context.Response.StatusCode = StatusCodes.Status500InternalServerError;
                    context.Response.ContentType = "application/json";
                    IExceptionHandlerPathFeature error = context.Features.Get<IExceptionHandlerPathFeature>();
                    if (error != null)
                    {
                        System.Exception ex = error.Error;
                        bool isShow = false;

                        if (ex is CustomException)
                            isShow = true;

                        Response<NoContent> response = Response<NoContent>.Fail(
                            statusCode: StatusCodes.Status500InternalServerError,
                            isShow: isShow,
                            path: error.Path,
                            ex.Message
                            );

                        await context.Response.WriteAsync(JsonConvert.SerializeObject(response));
                    }

                });
            });
        }

        public static void UseDelayRequestDevelopment(this IApplicationBuilder app)
        {
            app.Use(async (context, next) =>
            {
                await Task.Delay(1000);
                await next.Invoke();
            });
        }
    }
}
