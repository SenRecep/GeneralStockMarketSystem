using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using GeneralStockMarket.ClientShared.ExtensionMethods;
using GeneralStockMarket.ClientShared.Settings;
using GeneralStockMarket.ClientShared.Settings.Interfaces;
using GeneralStockMarket.CoreLib.StringInfo;

using Microsoft.AspNetCore.Hosting;

namespace GeneralStockMarket.ClientShared.Helpers
{
    public class PhotoHelper
    {
        private readonly ServiceUrls currentServiceUrls;
        public PhotoHelper(IServicesBaseUrlSettings servicesBaseUrlSettings, IWebHostEnvironment environment)
        {
            currentServiceUrls = servicesBaseUrlSettings.GetServiceUrlsByEnvironment(environment);
        }
        public string GetProductImageUrl(string productImageName) =>
            $"{currentServiceUrls.WebApi}/{ImageInfo.ProductImages}/{productImageName}";
    }
}
