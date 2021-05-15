
using GeneralStockMarket.ClientShared.Settings.Interfaces;

namespace GeneralStockMarket.ClientShared.Settings
{
    public class ClientSettings : IClientSettings
    {
        public Client WebClient { get; set; }
        public Client WebClientForUser { get; set; }
        public GrantType GrantType { get; set; }
    }
    public class Client
    {
        public string ClientId { get; set; }
        public string ClientSecret { get; set; }
    }
    public class GrantType
    {
        public string ClientCredential { get; set; }
        public string ResourceOwnerPasswordCredential { get; set; }
        public string RefreshTokenCredential { get; set; }
    }
}
