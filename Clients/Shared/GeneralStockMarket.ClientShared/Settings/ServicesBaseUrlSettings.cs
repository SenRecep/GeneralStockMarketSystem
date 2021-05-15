
using GeneralStockMarket.ClientShared.Settings.Interfaces;

namespace GeneralStockMarket.ClientShared.Settings
{
    public class ServicesBaseUrlSettings : IServicesBaseUrlSettings
    {
        public ServiceUrls Local { get; set; }
        public ServiceUrls Server { get; set; }
    }
    public class ServiceUrls
    {
        public string WebApi { get; set; }
        public string IdentityServer { get; set; }
    }
}
