﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using GeneralStockMarket.Dal.Concrete.EntityFrameworkCore.Contexts;
using GeneralStockMarket.Dal.Interface;
using GeneralStockMarket.Entities.Concrete;

using Microsoft.EntityFrameworkCore;

namespace GeneralStockMarket.Dal.Concrete.EntityFrameworkCore.Repositories
{
    public class EfProductDepositRequestRepository : IProductDepositRequestRepository
    {
        private readonly GeneralStockMarketDbContext dbContext;

        public EfProductDepositRequestRepository(GeneralStockMarketDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public async Task<IEnumerable<ProductDepositRequest>> GetAllByUserIdWhitAsync(Guid id) => 
            await dbContext.ProductDepositRequests.Where(x => !x.IsDeleted && x.UserId == id).Include(x => x.Product).ToListAsync();
    }
}
