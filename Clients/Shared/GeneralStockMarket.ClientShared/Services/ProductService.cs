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
    }
}
