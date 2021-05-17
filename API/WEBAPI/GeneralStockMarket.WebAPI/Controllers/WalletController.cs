using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Threading.Tasks;

using GeneralStockMarket.ApiShared.ControllerBases;
using GeneralStockMarket.ApiShared.Services.Interfaces;
using GeneralStockMarket.Bll.Interfaces;
using GeneralStockMarket.CoreLib.ExtensionMethods;
using GeneralStockMarket.CoreLib.Response;
using GeneralStockMarket.DTO.Wallet;
using GeneralStockMarket.Entities.Concrete;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace GeneralStockMarket.WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class WalletController : CustomControllerBase
    {
        private readonly IGenericService<Wallet> walletGenericService;
        private readonly IWalletService walletService;
        private readonly ISharedIdentityService sharedIdentityService;
        private readonly ILogger<WalletController> logger;

        public WalletController(
        IGenericService<Wallet> walletGenericService,
        IWalletService walletService,
         ISharedIdentityService sharedIdentityService,
         ILogger<WalletController> logger)
        {
            this.walletGenericService = walletGenericService;
            this.walletService = walletService;
            this.sharedIdentityService = sharedIdentityService;
            this.logger = logger;
            this.logger = logger;
        }
        ///<summary>
        ///Kullanıcının cüzdanının getirilmesi.
        ///</summary>  
        ///<response code="200">Başarıyla getirildi.</response>
        [HttpGet]
        public async Task<IActionResult> Get()
        {
            var userId = Guid.Parse(sharedIdentityService.GetUserId);
            var wallet = await walletService.GetWalletByUserIdAsync(userId);
            var response = Response<WalletDto>.Success(wallet, StatusCodes.Status200OK);
            logger.LogResponse(response, "Cüzdan başarıyla geldi.");
            return CreateResponseInstance(response);
        }

        ///<summary>
        ///Kullanıcının cüzdanının oluşturulması ve getirilmesi.
        ///</summary>  
        ///<response code="201">Başarıyla oluşturuldu/getirildi.</response>
        ///<response code="500">Cüzdan oluşturulurken bir hata ile karşılaşıldı</response>
        [HttpGet("CreateWallet")]
        public async Task<IActionResult> CreateWallet()
        {
            Response<NoContent> response = null;
            var userId = Guid.Parse(sharedIdentityService.GetUserId);
            var isExist = await walletService.WalletIsExistByUserIdAsync(userId);
            if (!isExist)
            {
                WalletCreateDto createWallet = new()
                {
                    CreatedUserId = userId,
                    UserId = userId
                };

                var wallet = await walletGenericService.AddAsync(createWallet);
                await walletGenericService.Commit();
                if (wallet is null)
                {
                    response = Response<NoContent>.Fail(
                           statusCode: StatusCodes.Status500InternalServerError,
                            isShow: false,
                            path: "api/wallet/cratewallet",
                            errors: "Cüzdan oluşturulurken bir hata ile karşılaşıldı"
                        );
                    logger.LogResponse(response, "Cüzdan oluşturulamadı.");
                    return CreateResponseInstance(response);
                }


            }
            response = Response<NoContent>.Success(StatusCodes.Status201Created);
            logger.LogResponse(response, "Cüzdan başarıyla oluşturuldu/getirildi.");
            return CreateResponseInstance(response);
        }
    }
}
