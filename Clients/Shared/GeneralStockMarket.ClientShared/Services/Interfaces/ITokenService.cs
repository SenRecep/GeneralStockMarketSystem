using System.Threading.Tasks;

using GeneralStockMarket.ClientShared.Models;
using GeneralStockMarket.CoreLib.Response;

namespace GeneralStockMarket.ClientShared.Services.Interfaces
{
    public interface ITokenService
    {
        public Task<Response<string>> ConnectTokenAsync();
        public Task<Response<Token>> RefreshTokenAsync();
        public Task<Response<string>> RevokeRefreshTokenAsync();
    }
}
