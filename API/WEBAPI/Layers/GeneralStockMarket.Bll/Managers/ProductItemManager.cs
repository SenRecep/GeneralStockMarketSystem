using System;
using System.Threading.Tasks;

using GeneralStockMarket.Bll.Interfaces;
using GeneralStockMarket.Bll.StringInfos;
using GeneralStockMarket.Dal.Interface;
using GeneralStockMarket.Entities.Concrete;

namespace GeneralStockMarket.Bll.Managers
{
    public class ProductItemManager : IProductItemService
    {
        private readonly IProductItemRepository productItemRepository;
        private readonly IGenericRepository<ProductItem> productItemGenericRepository;

        public ProductItemManager(IProductItemRepository productItemRepository,IGenericRepository<ProductItem> productItemGenericRepository)
        {
            this.productItemRepository = productItemRepository;
            this.productItemGenericRepository = productItemGenericRepository;
        }

        public async Task<ProductItem> GetAsync(Guid walletId, Guid productId)
        {
            var productItem = await productItemRepository.GetByProductIdWhitWalletIdAsync(walletId, productId);
            if (productItem == null)
            {
                productItem = new ProductItem()
                {
                    CreatedUserId = Guid.Parse(UserStringInfo.SystemUserId),
                    ProductId=productId,
                    WalletId=walletId
                };
                await productItemGenericRepository.AddAsync(productItem);
                await productItemGenericRepository.Commit();
            }
            return productItem;
        }

        public async Task<ProductItem> GetByProductIdWithWalletIdAsync(Guid walletId, Guid productId)
        {
            return await productItemRepository.GetByProductIdWhitWalletIdAsync(walletId, productId);
        }
    }
}
