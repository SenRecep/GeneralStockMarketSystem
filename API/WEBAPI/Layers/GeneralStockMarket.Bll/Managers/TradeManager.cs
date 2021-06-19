
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using AutoMapper;

using GeneralStockMarket.Bll.Interfaces;
using GeneralStockMarket.Bll.Models;
using GeneralStockMarket.Bll.StringInfos;
using GeneralStockMarket.CoreLib.ExtensionMethods;
using GeneralStockMarket.CoreLib.Response;
using GeneralStockMarket.Dal.Interface;
using GeneralStockMarket.DTO.MerketItem;
using GeneralStockMarket.DTO.Transaction;
using GeneralStockMarket.DTO.Wallet;
using GeneralStockMarket.Entities.Concrete;

using Microsoft.AspNetCore.Http;

namespace GeneralStockMarket.Bll.Managers
{
    public class TradeManager : ITradeService
    {
        const double ComissionRate = 0.01;

        private readonly ITradeRepository tradeRepository;
        private readonly IGenericRepository<ProductItem> genericProductItemRepository;
        private readonly IProductService productService;
        private readonly IMapper mapper;
        private readonly IGenericService<Wallet> genericWalletService;
        private readonly IGenericService<Transaction> genericTransactionService;
        private readonly IGenericService<MarketItem> genericMarketItemService;
        private readonly IGenericRepository<MarketItem> genericMarketItemRepository;

        public TradeManager(
            ITradeRepository tradeRepository,
         IGenericRepository<ProductItem> genericProductItemRepository,
         IProductService productService,
         IMapper mapper,
         IGenericService<Wallet> genericWalletService,
         IGenericService<Transaction> genericTransactionService,
         IGenericService<MarketItem> genericMarketItemService,
        IGenericRepository<MarketItem> genericMarketItemRepository)
        {
            this.tradeRepository = tradeRepository;
            this.genericProductItemRepository = genericProductItemRepository;
            this.productService = productService;
            this.mapper = mapper;
            this.genericWalletService = genericWalletService;
            this.genericTransactionService = genericTransactionService;
            this.genericMarketItemService = genericMarketItemService;
            this.genericMarketItemRepository = genericMarketItemRepository;
        }

        public async Task<Response<NoContent>> BuyAsync(BuyModel buyModel)
        {
            Response<NoContent> response = null;
            DTO.Product.ProductTradeDto productTradeDto = await productService.GetProductByIdAsync(buyModel.ProductId);
            if (productTradeDto.Amount < buyModel.Amount)
            {
                response = Response<NoContent>.Fail(
                    statusCode: StatusCodes.Status400BadRequest,
                    isShow: true,
                    path: "[post] api/trade",
                    errors: "Piyasada bu miktarda ürün bulunmamaktadır"
                    );
                return response;
            }

            List<MarketItemDto> marketItems = productTradeDto.MarketItems.Where(x => x.WalletId != buyModel.WalletId).OrderBy(x => x.UnitPrice).ToList();

            double count = 0;
            if (marketItems != null && marketItems.Any())
            {
                foreach (MarketItemDto marketItem in marketItems)
                {
                    if (count == buyModel.Amount)
                        break;
                    double buyToCount = buyModel.Amount - count;
                    double satinalinabilecekitemsayisi = buyModel.WalletDto.Money / (marketItem.UnitPrice * (1 + ComissionRate));
                    double satinalinacaksayisi = 0;
                    if (buyToCount <= satinalinabilecekitemsayisi)
                    {
                        if (buyToCount >= marketItem.Amount)
                            satinalinacaksayisi = marketItem.Amount;
                        else if (marketItem.Amount > buyToCount)
                            satinalinacaksayisi = buyToCount;
                    }
                    else
                        satinalinacaksayisi = satinalinabilecekitemsayisi;

                    try
                    {
                        count += satinalinacaksayisi;
                        //Kullanicinin hesabindan satin alinacak urunun parasini dus
                        buyModel.WalletDto.Money -= satinalinacaksayisi * (marketItem.UnitPrice * (1 + ComissionRate));
                        await genericWalletService.UpdateAsync(new WalletUpdateDto()
                        {
                            Id = buyModel.WalletDto.Id,
                            Money = buyModel.WalletDto.Money,
                            UpdateUserId = Guid.Parse(UserStringInfo.SystemUserId)
                        });
                        //satan kullaniciya parayi ver
                        WalletUpdateDto sellerWallet = await genericWalletService.GetByIdAsync<WalletUpdateDto>(marketItem.WalletId);
                        sellerWallet.Money += satinalinacaksayisi * marketItem.UnitPrice;
                        sellerWallet.UpdateUserId = Guid.Parse(UserStringInfo.SystemUserId);
                        await genericWalletService.UpdateAsync(sellerWallet);

                        //Accounting UPDATE
                        WalletUpdateDto accountingWallet = await genericWalletService.GetByIdAsync<WalletUpdateDto>(AccountingState.AccountingWalletId);
                        accountingWallet.Money += satinalinacaksayisi * marketItem.UnitPrice * ComissionRate;
                        accountingWallet.UpdateUserId = Guid.Parse(UserStringInfo.SystemUserId);
                        await genericWalletService.UpdateAsync(accountingWallet);
                        await genericWalletService.Commit();

                        //kullanicinin hesabindaki urun adedini guncelle
                        buyModel.ProductItem.Amount += satinalinacaksayisi;
                        buyModel.ProductItem.UpdateUserId = Guid.Parse(UserStringInfo.SystemUserId);
                        await genericProductItemRepository.UpdateAsync(buyModel.ProductItem);
                        await genericProductItemRepository.Commit();
                        //marketItem urun adedini guncelle
                        marketItem.Amount -= satinalinacaksayisi;
                        if (marketItem.Amount == 0)
                            marketItem.InProgress = false;
                        MarketItemUpdateDto marketItemUpdateDto = mapper.Map<MarketItemUpdateDto>(marketItem);
                        marketItemUpdateDto.UpdateUserId = Guid.Parse(UserStringInfo.SystemUserId);
                        await genericMarketItemService.UpdateAsync(marketItemUpdateDto);
                        await genericMarketItemService.Commit();
                        //islem gecmisi olustur
                        TransactionCreateDto transaction = new()
                        {
                            Amount = satinalinacaksayisi,
                            CreatedUserId = Guid.Parse(UserStringInfo.SystemUserId),
                            ProductId = buyModel.ProductId,
                            UnitPrice = marketItem.UnitPrice,
                            WalletIdBuyer = buyModel.WalletId,
                            WalletIdSeller = sellerWallet.Id
                        };
                        await genericTransactionService.AddAsync(transaction);
                        await genericTransactionService.Commit();
                        response = Response<NoContent>.Success(StatusCodes.Status201Created);
                    }
                    catch
                    {
                        await genericWalletService.Commit(false);
                        await genericProductItemRepository.Commit(false);
                        await genericMarketItemService.Commit(false);
                        await genericTransactionService.Commit(false);
                        response = Response<NoContent>.Fail(
                              statusCode: StatusCodes.Status500InternalServerError,
                              isShow: true,
                              path: "[post] api/trade",
                              errors: "Satin alim gerceklesirken bir hata meydana geldi"
                              );
                    }

                }
            }

            if (response == null)
                response = Response<NoContent>.Success(StatusCodes.Status201Created);
            return response;
        }

        public (ProductItem ProductItem, MarketItem MarketItem) CreateMarketItem(SellModel sellModel)
        {
            if (sellModel.Amount > sellModel.ProductItem.Amount)
                return new(sellModel.ProductItem, null);

            MarketItem marketItem = new()
            {
                Amount = sellModel.Amount,
                CreatedUserId = sellModel.UserId,
                InProgress = true,
                ProductId = sellModel.ProductId,
                UnitPrice = sellModel.UnitPrice,
                WalletId = sellModel.WalletId
            };
            sellModel.ProductItem.Amount -= sellModel.Amount;
            return new(sellModel.ProductItem, marketItem);
        }

        public async Task<Response<NoContent>> SellAsync(SellModel sellModel)
        {
            (ProductItem ProductItem, MarketItem MarketItem) sell = CreateMarketItem(sellModel);
            if (sell.MarketItem.IsNull())
            {
                Response<NoContent> response = Response<NoContent>.Fail(
                    statusCode: StatusCodes.Status400BadRequest,
                    isShow: true,
                    path: "[Post] api/trade",
                    errors: "Satma islemi iptal edildi, yeteri kadarda ürününüz mevcut değil"
                    );
                return response;
            }
            try
            {
                await genericMarketItemRepository.AddAsync(sell.MarketItem);
                await genericMarketItemRepository.Commit();
                sell.ProductItem.UpdateUserId = Guid.Parse(UserStringInfo.SystemUserId);
                await genericProductItemRepository.UpdateAsync(sell.ProductItem);
                await genericProductItemRepository.Commit();
                Response<NoContent> response = Response<NoContent>.Success(StatusCodes.Status201Created);
                return response;
            }
            catch
            {
                await genericMarketItemRepository.Commit(false);
                await genericProductItemRepository.Commit(false);
                Response<NoContent> response = Response<NoContent>.Fail(
                   statusCode: StatusCodes.Status500InternalServerError,
                   isShow: true,
                   path: "[Post] api/trade",
                   errors: "Satma islemi gerçekleşirken bir hata ile karşılaşıldı ve işlem iptal edildi"
                   );
                return response;
            }
        }
    }
}
