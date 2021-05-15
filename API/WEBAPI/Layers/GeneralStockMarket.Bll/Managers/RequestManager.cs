using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using GeneralStockMarket.Bll.Interfaces;
using GeneralStockMarket.DTO.Request;
using GeneralStockMarket.Entities.Concrete;

namespace GeneralStockMarket.Bll.Managers
{
    public class RequestManager : IRequestService
    {
        private readonly IGenericService<ProductDepositRequest> genericProductRequestService;
        private readonly IGenericService<NewTypeRequest> genericNewTypeRequestService;
        private readonly IGenericService<DepositRequest> genericDepositRequestService;

        public RequestManager(
            IGenericService<ProductDepositRequest> genericProductRequestService,
            IGenericService<NewTypeRequest> genericNewTypeRequestService,
            IGenericService<DepositRequest> genericDepositRequestService)
        {
            this.genericProductRequestService = genericProductRequestService;
            this.genericNewTypeRequestService = genericNewTypeRequestService;
            this.genericDepositRequestService = genericDepositRequestService;
        }
        public async Task<RequestDto> GetRequestsAsync(Guid id)
        {
            var productRequests = await genericProductRequestService.GetAllByUserIdAsync<ProductDepositRequestDto>(id);
            var depositRequests = await genericDepositRequestService.GetAllByUserIdAsync<DepositRequestDto>(id);
            var newTypeRequests = await genericNewTypeRequestService.GetAllByUserIdAsync<NewTypeRequestDto>(id);
            return new()
            {
                DepositRequestDtos = depositRequests,
                NewTypeRequestDtos = newTypeRequests,
                ProductDepositRequestDtos = productRequests
            };
        }
    }
}
