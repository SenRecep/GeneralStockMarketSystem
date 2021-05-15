using System;
using System.Collections.Generic;
using System.Threading.Tasks;

using GeneralStockMarket.DTO.Product;

namespace GeneralStockMarket.Bll.Interfaces
{
    public interface IProductService
    {
        public Task<List<ProductDto>> GetProductsAsync();
        public Task<ProductTradeDto> GetProductByIdAsync(Guid id);
    }
}
