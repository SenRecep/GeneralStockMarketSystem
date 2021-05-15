using System;
using System.Linq;
using System.Threading.Tasks;

using GeneralStockMarket.Dal.Concrete.EntityFrameworkCore.Contexts;
using GeneralStockMarket.Dal.Interface;
using GeneralStockMarket.Entities.Concrete;

using Microsoft.EntityFrameworkCore;

namespace GeneralStockMarket.Dal.Concrete.EntityFrameworkCore.Repositories
{
    public class EfWalletRepository : IWalletRepository
    {
        private readonly GeneralStockMarketDbContext dbContext;

        public EfWalletRepository(GeneralStockMarketDbContext dbContext)
        {
            this.dbContext = dbContext;
        }
        public async Task<Wallet> GetWalletByUserIdAsync(Guid id)
        {
            return await dbContext.Wallets
                .Include(x => x.ProductItems.Where(p => !p.IsDeleted))
                .Include(x => x.MarketItems.Where(m => !m.IsDeleted))
                .FirstOrDefaultAsync(x => x.UserId == id);
        }
        public async Task<bool> WalletIsExistByUserIdAsync(Guid id)
        {
            var wallet= await dbContext.Wallets
                .FirstOrDefaultAsync(x => x.UserId == id);
            return wallet is not null;
        }
    }
}
