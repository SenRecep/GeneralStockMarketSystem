using System;
using System.Collections.Generic;
using System.Threading.Tasks;

using GeneralStockMarket.Bll.Interfaces;
using GeneralStockMarket.Dal.Interface;
using GeneralStockMarket.Entities.Concrete;

namespace GeneralStockMarket.Bll.Managers
{
    public class ProductDepositRequestManager : IProductDepositRequestService
    {
        private readonly IProductDepositRequestRepository productDepositRequestRepository;

        public ProductDepositRequestManager(IProductDepositRequestRepository productDepositRequestRepository)
        {
            this.productDepositRequestRepository = productDepositRequestRepository;
        }

        public async Task<IEnumerable<ProductDepositRequest>> GetAllByUserIdWhitAsync(Guid id) =>
          await productDepositRequestRepository.GetAllByUserIdWhitAsync(id);

        public async Task<IEnumerable<ProductDepositRequest>> GetAllIncludeProductAsync() =>
          await productDepositRequestRepository.GetAllIncludeProductAsync();

        public async Task<ProductDepositRequest> GetByProductIdWhitUserIdAsync(Guid userId, Guid productId) =>
            await productDepositRequestRepository.GetByProductIdWhitUserIdAsync(userId, productId);
    }
}
