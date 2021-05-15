using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;

using GeneralStockMarket.ClientShared.ExtensionMethods;
using GeneralStockMarket.ClientShared.Services.Interfaces;
using GeneralStockMarket.CoreLib.Response;
using GeneralStockMarket.DTO.User;

namespace GeneralStockMarket.ClientShared.Services
{
    public class UserService : IUserService
    {
        private readonly HttpClient httpClient;

        public UserService(HttpClient httpClient) => this.httpClient = httpClient;

        public async Task<Response<UserDto>> GetUserAsync() => await httpClient.GetFromJsonAsync<Response<UserDto>>("api/user/getuser");

        public async Task<Response<NoContent>> UpdateProfileAsync(UserDto dto)
        {
            var res = await httpClient.PutAsJsonAsync("api/user/updateprofile", dto);
            if (!res.IsSuccessStatusCode)
                return await res.GetResponseAsync<NoContent>(false, "UserService/UpdateProfileAsync", "Kullanıcı güncellenirken edilirken beklenmedik bir hata ile karşılaşıldı");
            return await res.Content.ReadFromJsonAsync<Response<NoContent>>();
        }
    }
}
