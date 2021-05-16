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
        public async Task<Response<RequestDto>> GetRequestsAsync()
        {
            return await httpClient.GetFromJsonAsync<Response<RequestDto>>("api/request");
        }

        public async Task<Response<NoContent>> PostRequestAsync(GenaralCreateDto dto)
        {
            var httpResponse = await httpClient.PostAsJsonAsync("api/request", dto);
            return  await httpResponse.Content.ReadFromJsonAsync<Response<NoContent>>();
        }
    }
}
