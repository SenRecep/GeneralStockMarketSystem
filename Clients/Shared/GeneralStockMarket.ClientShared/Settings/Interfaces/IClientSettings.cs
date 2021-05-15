namespace GeneralStockMarket.ClientShared.Settings.Interfaces
{
    public interface IClientSettings
    {
        public Client WebClient { get; set; }
        public Client WebClientForUser { get; set; }
        public GrantType GrantType { get; set; }
    }
  
}
