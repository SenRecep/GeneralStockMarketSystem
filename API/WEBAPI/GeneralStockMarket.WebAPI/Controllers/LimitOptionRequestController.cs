using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using AutoMapper;

using GeneralStockMarket.ApiShared.ControllerBases;
using GeneralStockMarket.ApiShared.Services.Interfaces;
using GeneralStockMarket.CoreLib.Response;
using GeneralStockMarket.Dal.Interface;
using GeneralStockMarket.DTO.Request;
using GeneralStockMarket.Entities.Concrete;

using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace GeneralStockMarket.WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LimitOptionRequestController : CustomControllerBase
    {
        private readonly IMapper mapper;
        private readonly ISharedIdentityService sharedIdentityService;
        private readonly ILimitOptionRequestRepository limitOptionRequestRepository;
        private readonly IGenericRepository<LimitOptionRequest> genericLimitOptionRepository;
        private readonly IGenericRepository<Wallet> genericWalletRepository;

        public LimitOptionRequestController
            (IMapper mapper,
            ISharedIdentityService sharedIdentityService,
            ILimitOptionRequestRepository limitOptionRequestRepository,
            IGenericRepository<LimitOptionRequest> genericLimitOptionRepository,
            IGenericRepository<Wallet> genericWalletRepository)
        {
            this.mapper = mapper;
            this.sharedIdentityService = sharedIdentityService;
            this.limitOptionRequestRepository = limitOptionRequestRepository;
            this.genericLimitOptionRepository = genericLimitOptionRepository;
            this.genericWalletRepository = genericWalletRepository;
        }
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var requests = await limitOptionRequestRepository.GetAllAsync(Guid.Parse(sharedIdentityService.GetUserId));
            requests = requests.OrderByDescending(x=>x.InProgress).ThenByDescending(x=>x.CreatedTime).ToList();
            var result = mapper.Map<List<LimitRequestDto>>(requests);
            return CreateResponseInstance(Response<List<LimitRequestDto>>.Success(result, StatusCodes.Status200OK));
        }
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteLimitOption(Guid id)
        {
            var request = await limitOptionRequestRepository.GetAsync(id);
            request.InProgress = false;
            request.Wallet.Money += request.Money;
            request.Money = 0;
            await genericWalletRepository.UpdateAsync(request.Wallet);
            await genericWalletRepository.Commit();
            await genericLimitOptionRepository.UpdateAsync(request);
            await genericLimitOptionRepository.Commit();
            return CreateResponseInstance(Response<NoContent>.Success(StatusCodes.Status204NoContent));
        }

    }
}
