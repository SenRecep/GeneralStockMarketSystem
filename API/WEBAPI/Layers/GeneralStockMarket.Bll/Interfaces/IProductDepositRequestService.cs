using System;
using System.Collections.Generic;
using System.Threading.Tasks;

using GeneralStockMarket.Entities.Concrete;

namespace GeneralStockMarket.Bll.Interfaces
{
    public interface IProductDepositRequestService
    {
        public Task<IEnumerable<ProductDepositRequest>> GetAllByUserIdWhitAsync(Guid id);

    }
}
