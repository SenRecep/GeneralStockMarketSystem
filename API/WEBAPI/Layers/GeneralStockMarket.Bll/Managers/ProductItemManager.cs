using System;
using System.Threading.Tasks;

using GeneralStockMarket.Bll.Interfaces;
using GeneralStockMarket.Dal.Interface;
using GeneralStockMarket.Entities.Concrete;

namespace GeneralStockMarket.Bll.Managers
{
    public class ProductItemManager : IProductItemService
    {
        private readonly IProductItemRepository productItemRepository;

        public ProductItemManager(IProductItemRepository productItemRepository)
        {
            this.productItemRepository = productItemRepository;
        }
        public async Task<ProductItem> GetByProductIdWhitWalletIdAsync(Guid walletId, Guid productId)
        {
            return await productItemRepository.GetByProductIdWhitWalletIdAsync(walletId, productId);
        }
    }
}
