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

namespace GeneralStockMarket.ClientShared.Services
{
    public class ProductService : IProductService
    {
        private readonly HttpClient httpClient;

        public ProductService(HttpClient httpClient)
        {
            this.httpClient = httpClient;
        }

        public async Task<Response<ProductTradeDto>> GetProductByIdAsync(Guid id) =>
             await httpClient.GetFromJsonAsync<Response<ProductTradeDto>>($"api/product/{id}");

        public async Task<Response<IEnumerable<ProductDto>>> GetProductsAsync() =>
             await httpClient.GetFromJsonAsync<Response<IEnumerable<ProductDto>>>("api/product");

        public async Task<Response<ProductDto>> PostProductsAsync(ProductCreateClientDto dto)
        {
            var content = new MultipartFormDataContent();
            content.Headers.ContentDisposition = new System.Net.Http.Headers.ContentDispositionHeaderValue("form-data");
            content.Add(new StreamContent(dto.Image, (int)dto.Image.Length), "Image",dto.FileName);
            var productName = new StringContent(dto.Name, System.Text.Encoding.UTF8);
            content.Add(productName, "Name");
            var response= await httpClient.PostAsync("api/product",content);
            return await response.Content.ReadFromJsonAsync<Response<ProductDto>>();
        }
    }
}
