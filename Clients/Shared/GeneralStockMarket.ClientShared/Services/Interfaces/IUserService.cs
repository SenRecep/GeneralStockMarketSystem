using System.Threading.Tasks;

using GeneralStockMarket.CoreLib.Response;
using GeneralStockMarket.DTO.User;

namespace GeneralStockMarket.ClientShared.Services.Interfaces
{
    public interface IUserService
    {
        public Task<Response<UserDto>> GetUserAsync();
        public Task<Response<NoContent>> UpdateProfileAsync(UserDto dto);
    }
}
