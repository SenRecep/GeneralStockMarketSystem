using System;
using System.Threading.Tasks;

using GeneralStockMarket.Entities.Concrete;

namespace GeneralStockMarket.Bll.Interfaces
{
    public interface IProductItemService
    {
        public Task<ProductItem> GetByProductIdWhitWalletIdAsync(Guid walletId, Guid productId);
    }
}
