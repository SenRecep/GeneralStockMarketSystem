
using System.Text.Json;

using Blazored.LocalStorage;

using GeneralStockMarket.ClientShared.Containers.MicrosoftIOC;

using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace GeneralStockMarket.BlazorServerAppClient
{
    public class Startup
    {
        private readonly IConfiguration configuration;
        private readonly IWebHostEnvironment environment;

        public Startup(IConfiguration configuration, IWebHostEnvironment environment)
        {
            this.configuration = configuration;
            this.environment = environment;
        }


        public void ConfigureServices(IServiceCollection services)
        {
            services.AddDependencies(configuration, environment);

            services.AddBlazoredLocalStorage(config =>
            {
                //config.JsonSerializerOptions.DictionaryKeyPolicy = JsonNamingPolicy.CamelCase;
                //config.JsonSerializerOptions.IgnoreNullValues = true;
                //config.JsonSerializerOptions.IgnoreReadOnlyProperties = true;
                //config.JsonSerializerOptions.PropertyNameCaseInsensitive = true;
                //config.JsonSerializerOptions.PropertyNamingPolicy = JsonNamingPolicy.CamelCase;
                //config.JsonSerializerOptions.ReadCommentHandling = JsonCommentHandling.Skip;
                config.JsonSerializerOptions.WriteIndented = false;
            });

            services.AddControllersWithViews().AddValidationDependencies();
            services.AddRazorPages().AddValidationDependencies();
            services.AddServerSideBlazor();
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Error");
                app.UseHsts();
            }

            app.UseStaticFiles();

            app.UseRouting();

            app.UseCookiePolicy();

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapDefaultControllerRoute();
                endpoints.MapBlazorHub();
                endpoints.MapFallbackToPage("/_Host");
            });
        }
    }
}
