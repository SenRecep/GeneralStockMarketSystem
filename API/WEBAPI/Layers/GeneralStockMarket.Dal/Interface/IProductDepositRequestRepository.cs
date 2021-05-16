using System;
using System.Collections.Generic;
using System.Threading.Tasks;

using GeneralStockMarket.Entities.Concrete;

namespace GeneralStockMarket.Dal.Interface
{
    public interface IProductDepositRequestRepository
    {
        public Task<IEnumerable<ProductDepositRequest>> GetAllByUserIdWhitAsync(Guid id);
    }
}
