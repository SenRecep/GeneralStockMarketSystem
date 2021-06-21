using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;

using GeneralStockMarket.ClientShared.Services.Interfaces;
using GeneralStockMarket.CoreLib.Response;
using GeneralStockMarket.DTO.Product;
using GeneralStockMarket.DTO.Request;
using GeneralStockMarket.DTO.Request.Enums;

using Microsoft.AspNetCore.Http;

namespace GeneralStockMarket.ClientShared.Services
{
    public class RequestService : IRequestService
    {
        private readonly HttpClient httpClient;

        public RequestService(HttpClient httpClient)
        {
            this.httpClient = httpClient;
        }

        public async Task<Response<NoContent>> DeleteRequestAsync(RequestType type, Guid id)
        {
            var httpResponse = await httpClient.DeleteAsync($"api/request/{type}/{id}");
            return await httpResponse.Content.ReadFromJsonAsync<Response<NoContent>>();
        }

        public Task<Response<List<LimitRequestDto>>> GetAllLimitRequestAsync()
        {
            return httpClient.GetFromJsonAsync<Response<List<LimitRequestDto>>>("api/limitOptionRequest");
        }

        public async Task<Response<RequestDto>> GetAllRequestsAsync()
        {
            return await httpClient.GetFromJsonAsync<Response<RequestDto>>("api/request/getall");
        }

        public async Task<Response<NewTypeRequestDto>> GetNewTypeRequestAsync(Guid id)
        {
            return await httpClient.GetFromJsonAsync<Response<NewTypeRequestDto>>($"api/request/getnewtyperequest/{id}");
        }

        public async Task<Response<RequestDto>> GetRequestsAsync()
        {
            return await httpClient.GetFromJsonAsync<Response<RequestDto>>("api/request");
        }

        public async Task<Response<NoContent>> PostRequestAsync(GenaralCreateDto dto)
        {
            var httpResponse = await httpClient.PostAsJsonAsync("api/request", dto);
            return await httpResponse.Content.ReadFromJsonAsync<Response<NoContent>>();
        }

        public async Task<Response<NoContent>> RemoveLimitOptionAsync(Guid id)
        {
            var httpResponse = await httpClient.DeleteAsync($"api/limitOptionRequest/{id}");
            return await httpResponse.Content.ReadFromJsonAsync<Response<NoContent>>();
        }

        public async Task<Response<NoContent>> VerifyUpdateRequestAsync(bool mode, RequestType type, Guid id)
        {
            var httpResponse = await httpClient.DeleteAsync($"api/request/verifyupdate/{mode}/{type}/{id}");
            return await httpResponse.Content.ReadFromJsonAsync<Response<NoContent>>();
        }
    }
}
