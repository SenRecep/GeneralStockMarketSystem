namespace GeneralStockMarket.ClientShared.Settings.Interfaces
{
    public interface IServicesBaseUrlSettings
    {
        public ServiceUrls Local { get; set; }
        public ServiceUrls Server { get; set; }
    }

}
