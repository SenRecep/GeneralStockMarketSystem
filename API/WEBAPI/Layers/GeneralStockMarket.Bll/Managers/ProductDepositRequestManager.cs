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

        public Task<IEnumerable<ProductDepositRequest>> GetAllByUserIdWhitAsync(Guid id) =>
            productDepositRequestRepository.GetAllByUserIdWhitAsync(id);
    }
}
