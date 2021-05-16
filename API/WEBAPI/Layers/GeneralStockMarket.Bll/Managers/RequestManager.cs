using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using AutoMapper;

using GeneralStockMarket.Bll.Interfaces;
using GeneralStockMarket.DTO.Request;
using GeneralStockMarket.Entities.Concrete;

namespace GeneralStockMarket.Bll.Managers
{
    public class RequestManager : IRequestService
    {
        private readonly IProductDepositRequestService ProductRequestService;
        private readonly IGenericService<NewTypeRequest> genericNewTypeRequestService;
        private readonly IGenericService<DepositRequest> genericDepositRequestService;
        private readonly IMapper mapper;

        public RequestManager(
            IProductDepositRequestService ProductRequestService,
            IGenericService<NewTypeRequest> genericNewTypeRequestService,
            IGenericService<DepositRequest> genericDepositRequestService,
            IMapper mapper)
        {
            this.ProductRequestService = ProductRequestService;
            this.genericNewTypeRequestService = genericNewTypeRequestService;
            this.genericDepositRequestService = genericDepositRequestService;
            this.mapper = mapper;
        }
        public async Task<RequestDto> GetRequestsAsync(Guid id)
        {
            var productRequests = await ProductRequestService.GetAllByUserIdWhitAsync(id);
            var depositRequests = await genericDepositRequestService.GetAllByUserIdAsync<DepositRequestDto>(id);
            var newTypeRequests = await genericNewTypeRequestService.GetAllByUserIdAsync<NewTypeRequestDto>(id);
            return new()
            {
                DepositRequestDtos = depositRequests,
                NewTypeRequestDtos = newTypeRequests,
                ProductDepositRequestDtos = mapper.Map<IEnumerable<ProductDepositRequestDto>>(productRequests)
            };
        }
    }
}
