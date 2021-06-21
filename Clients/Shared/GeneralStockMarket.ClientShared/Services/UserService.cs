using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;

using GeneralStockMarket.ClientShared.Services.Interfaces;
using GeneralStockMarket.CoreLib.Response;
using GeneralStockMarket.DTO.User;

namespace GeneralStockMarket.ClientShared.Services
{
    public class UserService : IUserService
    {
        private readonly HttpClient httpClient;

        public UserService(HttpClient httpClient) => this.httpClient = httpClient;


        public  Task<Response<UserDto>> GetUserAsync() =>  httpClient.GetFromJsonAsync<Response<UserDto>>("api/user/getuser");

        public async Task<Response<NoContent>> UpdateProfileAsync(UserDto dto)
        {
            HttpResponseMessage res = await httpClient.PutAsJsonAsync("api/user/updateprofile", dto);
            return await res.Content.ReadFromJsonAsync<Response<NoContent>>();
        }
    }
}
