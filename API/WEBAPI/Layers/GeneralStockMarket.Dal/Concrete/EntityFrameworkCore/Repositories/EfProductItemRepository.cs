using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using GeneralStockMarket.Dal.Concrete.EntityFrameworkCore.Contexts;
using GeneralStockMarket.Dal.Interface;
using GeneralStockMarket.Entities.Concrete;

using Microsoft.EntityFrameworkCore;

namespace GeneralStockMarket.Dal.Concrete.EntityFrameworkCore.Repositories
{
    public class EfProductItemRepository : IProductItemRepository
    {
        private readonly GeneralStockMarketDbContext dbContext;

        public EfProductItemRepository(GeneralStockMarketDbContext dbContext)
        {
            this.dbContext = dbContext;
        }
        public async Task<ProductItem> GetByProductIdWhitWalletIdAsync(Guid walletId, Guid productId)
        {
            return await dbContext.ProductItems.FirstOrDefaultAsync(x=>!x.IsDeleted && x.WalletId==walletId && x.ProductId==productId);
        }
    }
}
