using System.Threading.Tasks;

using GeneralStockMarket.CoreLib.Response;
using GeneralStockMarket.DTO.Wallet;

namespace GeneralStockMarket.ClientShared.Services.Interfaces
{
    public interface IWalletService
    {
        public Task<Response<NoContent>> CreateWalletAsync();
        public Task<Response<WalletDto>> GetWalletAsync();
    }
}
