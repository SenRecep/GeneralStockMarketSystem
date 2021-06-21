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
    public class EfLimitOptionRequestRepository : ILimitOptionRequestRepository
    {
        private readonly GeneralStockMarketDbContext dbContext;

        public EfLimitOptionRequestRepository(GeneralStockMarketDbContext dbContext)
        {
            this.dbContext = dbContext;
        }
        public Task<List<LimitOptionRequest>> GetAllAsync(Guid userId)=>
             dbContext.LimitOptionRequests.Include(x => x.Product).Include(x=>x.Wallet).Where(x=>x.Wallet.UserId==userId).ToListAsync();

        public Task<LimitOptionRequest> GetAsync(Guid id)=>
            dbContext.LimitOptionRequests.Include(x => x.Wallet).FirstOrDefaultAsync(x => x.Id == id);
    }
}
