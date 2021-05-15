using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;

using GeneralStockMarket.ClientShared.Services.Interfaces;
using GeneralStockMarket.CoreLib.Response;
using GeneralStockMarket.DTO.Wallet;

namespace GeneralStockMarket.ClientShared.Services
{
    public class WalletService : IWalletService
    {
        private readonly HttpClient httpClient;

        public WalletService(HttpClient httpClient)
        {
            this.httpClient = httpClient;
        }
        public async Task<Response<NoContent>> CreateWalletAsync()
        {
            return await httpClient.GetFromJsonAsync<Response<NoContent>>("api/wallet/createwallet");
        }

        public async Task<Response<WalletDto>> GetWalletAsync()
        {
            return await httpClient.GetFromJsonAsync<Response<WalletDto>>("api/wallet");
        }
    }
}
