using System;
using System.Collections.Generic;
using System.Threading.Tasks;

using GeneralStockMarket.Entities.Concrete;

namespace GeneralStockMarket.Dal.Interface
{
    public interface IProductRepository
    {
        public Task<List<Product>> GetProductsAsync();
        public Task<Product> GetProductByIdAsync(Guid id);
    }
}
