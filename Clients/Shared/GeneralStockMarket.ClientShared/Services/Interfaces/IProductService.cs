using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

using GeneralStockMarket.CoreLib.Response;
using GeneralStockMarket.DTO.Product;

namespace GeneralStockMarket.ClientShared.Services.Interfaces
{
    public interface IProductService
    {
        public Task<Response<IEnumerable<ProductDto>>> GetProductsAsync();
        public Task<Response<ProductDto>> PostProductsAsync(ProductCreateClientDto dto);
        public Task<Response<ProductTradeDto>> GetProductByIdAsync(Guid id);
    }
}
