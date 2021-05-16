using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using GeneralStockMarket.Entities.Concrete;

namespace GeneralStockMarket.Dal.Interface
{
    public interface  IProductItemRepository
    {
        public Task<ProductItem> GetByProductIdWhitWalletIdAsync(Guid walletId, Guid productId);
    }
}
