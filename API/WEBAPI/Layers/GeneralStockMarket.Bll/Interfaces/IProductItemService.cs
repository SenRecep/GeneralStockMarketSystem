using System;
using System.Threading.Tasks;

using GeneralStockMarket.Entities.Concrete;

namespace GeneralStockMarket.Bll.Interfaces
{
    public interface IProductItemService
    {
        public Task<ProductItem> GetByProductIdWithWalletIdAsync(Guid walletId, Guid productId);
        public Task<ProductItem> GetAsync(Guid walletId, Guid productId);
    }
}
