using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;

using GeneralStockMarket.ClientShared.Models;
using GeneralStockMarket.ClientShared.ViewModels;
using GeneralStockMarket.CoreLib.Response;

namespace GeneralStockMarket.ClientShared.Services.Interfaces
{
    public interface IIdentityService
    {
        public Task<Response<IEnumerable<Claim>>> GetUserInfoAsync(string access_token);
        public Task<Response<Token>> RequestPasswordTokenAsync(LoginViewModel model);
        public Task<Response<NoContent>> UpdateUserClaimsAsync();
        public Task<Response<string>> SignInAsync(LoginViewModel model);
        public Task<Response<string>> RefreshTokenSignInAsync(Token token);
        public Task<Response<string>> SignUpAsync(RegisterViewModel model, string accessToken);
        public Task<Response<string>> SignOutAsync();

    }
}
