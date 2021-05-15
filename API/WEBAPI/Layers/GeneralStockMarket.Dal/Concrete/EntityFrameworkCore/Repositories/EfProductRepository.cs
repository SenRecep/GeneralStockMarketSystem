using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using GeneralStockMarket.Dal.Concrete.EntityFrameworkCore.Contexts;
using GeneralStockMarket.Dal.Interface;
using GeneralStockMarket.Entities.Concrete;

using Microsoft.EntityFrameworkCore;

namespace GeneralStockMarket.Dal.Concrete.EntityFrameworkCore.Repositories
{
    public class EfProductRepository : IProductRepository
    {
        private readonly GeneralStockMarketDbContext dbContext;

        public EfProductRepository(GeneralStockMarketDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public async Task<Product> GetProductByIdAsync(Guid id) =>
             await dbContext.Products.Include(x => x.MarketItems).FirstOrDefaultAsync(x => !x.IsDeleted && x.Id == id);

        public async Task<List<Product>> GetProductsAsync() =>
             await dbContext.Products.Include(x => x.MarketItems).Where(x => !x.IsDeleted).ToListAsync();
    }
}
