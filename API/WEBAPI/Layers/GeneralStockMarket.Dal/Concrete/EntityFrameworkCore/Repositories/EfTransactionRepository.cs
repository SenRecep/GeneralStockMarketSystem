using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using GeneralStockMarket.Dal.Concrete.EntityFrameworkCore.Contexts;
using GeneralStockMarket.Dal.Interface;
using GeneralStockMarket.Entities.ComplexTypes;

using Microsoft.EntityFrameworkCore;

namespace GeneralStockMarket.Dal.Concrete.EntityFrameworkCore.Repositories
{
    public class EfTransactionRepository : ITransactionRepository
    {
        private readonly GeneralStockMarketDbContext dbContext;

        public EfTransactionRepository(GeneralStockMarketDbContext dbContext)
        {
            this.dbContext = dbContext;
        }
        public Task<List<ExportModel>> GetAllAsync(Guid UserId) => dbContext.Transactions
                .Include(x => x.WalletBuyer)
                .Include(x => x.WalletSeller)
                .Include(x => x.Product)
                .Where(x => x.WalletSeller.UserId == UserId || x.WalletBuyer.UserId == UserId)
                .Select(x => new ExportModel($"{x.CreatedTime.ToLongDateString()} {x.CreatedTime.ToShortTimeString()}", x.Product.Name, x.UnitPrice, x.Amount, x.WalletSeller.UserId == UserId?"Satış":"Alış"))
                .ToListAsync();
    }
}
