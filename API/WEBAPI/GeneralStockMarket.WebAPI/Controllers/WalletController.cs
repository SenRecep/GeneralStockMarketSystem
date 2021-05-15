using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Threading.Tasks;

using GeneralStockMarket.ApiShared.ControllerBases;
using GeneralStockMarket.ApiShared.Services.Interfaces;
using GeneralStockMarket.Bll.Interfaces;
using GeneralStockMarket.CoreLib.Response;
using GeneralStockMarket.DTO.Wallet;
using GeneralStockMarket.Entities.Concrete;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace GeneralStockMarket.WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class WalletController : CustomControllerBase
    {
        private readonly IGenericService<Wallet> walletGenericService;
        private readonly IWalletService walletService;
        private readonly ISharedIdentityService sharedIdentityService;

        public WalletController(IGenericService<Wallet> walletGenericService,IWalletService walletService, ISharedIdentityService sharedIdentityService) 
        {
            this.walletGenericService = walletGenericService;
            this.walletService = walletService;
            this.sharedIdentityService = sharedIdentityService;
        }
        [HttpGet]
        public async Task<IActionResult> Get()
        {
            var userId = Guid.Parse(sharedIdentityService.GetUserId);
            var wallet = await walletService.GetWalletByUserIdAsync(userId);
            return CreateResponseInstance(Response<WalletDto>.Success(wallet, StatusCodes.Status200OK));
        }

        [HttpGet("CreateWallet")]
        public async Task<IActionResult> CreateWallet()
        {
            var userId=  Guid.Parse(sharedIdentityService.GetUserId); 
            var isExist = await walletService.WalletIsExistByUserIdAsync(userId);
            if (!isExist)
            {
                WalletCreateDto createWallet = new() {
                    CreatedUserId = userId,
                    UserId= userId
                };

               var wallet= await walletGenericService.AddAsync(createWallet);
                await walletGenericService.Commit();
                if (wallet is null)
                    return CreateResponseInstance(Response<NoContent>.Fail(
                           statusCode: StatusCodes.Status500InternalServerError,
                            isShow:false,
                            path:"api/wallet/cratewallet",
                            errors:"Cüzdan oluşturulurken bir hata ile karşılaşıldı"
                        ));

            }

            return CreateResponseInstance(Response<NoContent>.Success(StatusCodes.Status201Created));
        }
    }
}
