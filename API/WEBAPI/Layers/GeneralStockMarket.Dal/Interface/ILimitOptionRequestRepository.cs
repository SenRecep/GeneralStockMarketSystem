using System;
using System.Collections.Generic;
using System.Threading.Tasks;

using GeneralStockMarket.Entities.Concrete;

namespace GeneralStockMarket.Dal.Interface
{
    public interface ILimitOptionRequestRepository
    {
        public Task<List<LimitOptionRequest>> GetAllAsync(Guid userId);
        public Task<LimitOptionRequest> GetAsync(Guid id);
    }
}
