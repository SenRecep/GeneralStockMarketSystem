using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using AutoMapper;

using GeneralStockMarket.Bll.Interfaces;
using GeneralStockMarket.DTO.ProductItem;
using GeneralStockMarket.DTO.Request;
using GeneralStockMarket.DTO.Wallet;
using GeneralStockMarket.Entities.Concrete;

namespace GeneralStockMarket.Bll.Managers
{
    public class RequestManager : IRequestService
    {
        private readonly IProductDepositRequestService ProductRequestService;
        private readonly IProductItemService productItemService;
        private readonly IGenericService<ProductItem> genericProductItemService;
        private readonly IGenericService<NewTypeRequest> genericNewTypeRequestService;
        private readonly IGenericService<DepositRequest> genericDepositRequestService;
        private readonly IGenericService<ProductDepositRequest> genericProductDepositRequestService;
        private readonly IGenericService<Wallet> genericWalletService;
        private readonly IMapper mapper;

        public RequestManager(
            IProductDepositRequestService ProductRequestService,
            IProductItemService productItemService,
            IGenericService<ProductItem> genericProductItemService,
            IGenericService<NewTypeRequest> genericNewTypeRequestService,
            IGenericService<DepositRequest> genericDepositRequestService,
            IGenericService<ProductDepositRequest> genericProductDepositRequestService,
            IGenericService<Wallet> genericWalletService,
            IMapper mapper)
        {
            this.ProductRequestService = ProductRequestService;
            this.productItemService = productItemService;
            this.genericProductItemService = genericProductItemService;
            this.genericNewTypeRequestService = genericNewTypeRequestService;
            this.genericDepositRequestService = genericDepositRequestService;
            this.genericProductDepositRequestService = genericProductDepositRequestService;
            this.genericWalletService = genericWalletService;
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

        public async Task<RequestDto> GetAllRequestsAsync()
        {
            var productRequests = await ProductRequestService.GetAllIncludeProductAsync();
            var depositRequests = await genericDepositRequestService.GetAllAsync<DepositRequestDto>();
            var newTypeRequests = await genericNewTypeRequestService.GetAllAsync<NewTypeRequestDto>();
            return new()
            {
                DepositRequestDtos = depositRequests.Where(x => x.Verify == null).ToList(),
                NewTypeRequestDtos = newTypeRequests.Where(x => x.Verify == null).ToList(),
                ProductDepositRequestDtos = mapper.Map<IEnumerable<ProductDepositRequestDto>>(productRequests).Where(x => x.Verify == null).ToList()
            };
        }

        public async Task DepositRequestVerifyConditionAsync(VerifyDto dto)
        {
            if (dto.Verify.HasValue && dto.Verify.Value)
            {
                var entityDto = await genericDepositRequestService.GetByIdAsync<DepositRequestDto>(dto.Id);
                var walletDto = await genericWalletService.GetByUserIdAsync<WalletDto>(entityDto.CreatedUserId);
                walletDto.Money += entityDto.Amount;
                var updateDto = mapper.Map<WalletUpdateDto>(walletDto);
                updateDto.UpdateUserId = dto.UpdateUserId;
                await genericWalletService.UpdateAsync(updateDto);
                await genericWalletService.Commit();
            }
        }

        public async Task ProductDepositRequestVerifyConditionAsync(VerifyDto dto)
        {
            if (dto.Verify.HasValue && dto.Verify.Value)
            {
                var entityDto = await genericProductDepositRequestService.GetByIdAsync<ProductDepositRequestDto>(dto.Id);
                var walletDto = await genericWalletService.GetByUserIdAsync<WalletDto>(entityDto.CreatedUserId);
                var productItem = await productItemService.GetByProductIdWithWalletIdAsync(walletDto.Id, entityDto.ProductId);
                if (productItem == null)
                    productItem = await genericProductItemService.AddAsync(new ProductItemCreateDto()
                    {
                        CreatedUserId = entityDto.CreatedUserId, 
                        ProductId = entityDto.ProductId,
                        WalletId = walletDto.Id,
                        Amount = entityDto.Amount

                    });
                else
                {
                    productItem.Amount += entityDto.Amount;
                    productItem.UpdateUserId = dto.UpdateUserId;
                    var updateDto = mapper.Map<ProductItemUpdateDto>(productItem);
                    await genericProductItemService.UpdateAsync(updateDto);
                }
                await genericProductItemService.Commit();
            }
        }
    }
}
