
using GeneralStockMarket.ClientShared.Settings;
using GeneralStockMarket.ClientShared.Settings.Interfaces;

using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;

namespace GeneralStockMarket.ClientShared.ExtensionMethods
{
    public static class ServicesBaseUrlSettingExtensionMethods
    {
        public static ServiceUrls GetServiceUrlsByEnvironment(this IServicesBaseUrlSettings settings, IWebHostEnvironment environment) =>
            environment.IsDevelopment() ? settings.Local : settings.Server;

    }
}
