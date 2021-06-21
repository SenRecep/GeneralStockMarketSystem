using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using AutoMapper;

using GeneralStockMarket.ApiShared.ControllerBases;
using GeneralStockMarket.ApiShared.Services.Interfaces;
using GeneralStockMarket.Bll.Interfaces;
using GeneralStockMarket.Bll.Models;
using GeneralStockMarket.CoreLib.ExtensionMethods;
using GeneralStockMarket.CoreLib.Response;
using GeneralStockMarket.Dal.Interface;
using GeneralStockMarket.DTO.Trade;
using GeneralStockMarket.DTO.Wallet;
using GeneralStockMarket.Entities.Concrete;
using GeneralStockMarket.WebAPI.Services;

using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace GeneralStockMarket.WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TradeController : CustomControllerBase
    {
        private readonly ISharedIdentityService sharedIdentityService;
        private readonly IWalletService walletService;
        private readonly ITradeService tradeService;
        private readonly IProductItemService productItemService;

        private readonly IMapper mapper;

        public TradeController(
            ISharedIdentityService sharedIdentityService,
            IWalletService walletService,
            ITradeService tradeService,
            IProductItemService productItemService,

            IMapper mapper)
        {
            this.sharedIdentityService = sharedIdentityService;
            this.walletService = walletService;
            this.tradeService = tradeService;
            this.productItemService = productItemService;
            this.mapper = mapper;
        }
        [HttpPost]
        public async Task<IActionResult> Trade(TradeCreateDto tradeCreateDto)
        {
            Guid userId = Guid.Parse(sharedIdentityService.GetUserId);
            WalletDto walletDto = await walletService.GetWalletByUserIdAsync(userId);
            var productItem = await productItemService.GetAsync(walletDto.Id, tradeCreateDto.ProductId);
            if (tradeCreateDto.TradeType == DTO.General.TradeType.Sell)
            {
                var sellDto = mapper.Map<SellModel>(tradeCreateDto);
                sellDto.ProductItem = productItem;
                sellDto.WalletId = walletDto.Id;
                sellDto.UserId = userId;
                var response = await tradeService.SellAsync(sellDto);
                ScannerServiceState.IsScanned = true;
                return CreateResponseInstance(response);
            }
            else if(tradeCreateDto.TradeType==DTO.General.TradeType.Buy)
            {
                var buyDto=mapper.Map<BuyModel>(tradeCreateDto);
                buyDto.ProductItem = productItem;
                buyDto.WalletId = walletDto.Id;
                buyDto.UserId = userId;
                buyDto.WalletDto = walletDto;
                var response = await tradeService.BuyAsync(buyDto);
                ScannerServiceState.IsScanned = true;
                return CreateResponseInstance(response);
            }
            else
            {
                var buyDto = mapper.Map<BuybyLimitModel>(tradeCreateDto);
                buyDto.WalletId = walletDto.Id;
                buyDto.UserId = userId;
                var response = await tradeService.BuybyLimitAsync(buyDto);
                ScannerServiceState.IsScanned = true;
                return CreateResponseInstance(response);
            }
        }
    }
}
