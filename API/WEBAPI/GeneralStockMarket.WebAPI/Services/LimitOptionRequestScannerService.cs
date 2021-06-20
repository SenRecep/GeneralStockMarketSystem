using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

using AutoMapper;

using GeneralStockMarket.Bll.Interfaces;
using GeneralStockMarket.Bll.Models;
using GeneralStockMarket.Bll.StringInfos;
using GeneralStockMarket.Dal.Concrete.EntityFrameworkCore.Contexts;
using GeneralStockMarket.Dal.Interface;
using GeneralStockMarket.DTO.MerketItem;
using GeneralStockMarket.DTO.Transaction;
using GeneralStockMarket.DTO.Wallet;
using GeneralStockMarket.Entities.Concrete;

using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace GeneralStockMarket.WebAPI.Services
{
    public static class ScannerServiceState
    {
        public static bool IsScanned;
    }
    public class LimitOptionRequestScannerService : BackgroundService
    {
        const double ComissionRate = 0.01;
        private readonly IGenericRepository<LimitOptionRequest> limitOptionRequestGenericRepository;
        private readonly IGenericRepository<Wallet> walletGenericRepository;
        private readonly IGenericRepository<ProductItem> genericProductItemRepository;
        private readonly IMapper mapper;
        private readonly IGenericService<Wallet> genericWalletService;
        private readonly IGenericService<Transaction> genericTransactionService;
        private readonly IGenericService<MarketItem> genericMarketItemService;
        private readonly IGenericRepository<MarketItem> genericMarketItemRepository;
        private readonly IProductItemService productItemService;
        public LimitOptionRequestScannerService(IServiceScopeFactory serviceScopeFactory)
        {
            var scope = serviceScopeFactory.CreateScope();
            this.limitOptionRequestGenericRepository = scope.ServiceProvider.GetRequiredService<IGenericRepository<LimitOptionRequest>>();
            this.walletGenericRepository = scope.ServiceProvider.GetRequiredService<IGenericRepository<Wallet>>();
            this.genericProductItemRepository = scope.ServiceProvider.GetRequiredService<IGenericRepository<ProductItem>>();
            this.mapper = scope.ServiceProvider.GetRequiredService<IMapper>();
            this.genericWalletService = scope.ServiceProvider.GetRequiredService<IGenericService<Wallet>>();
            this.genericTransactionService = scope.ServiceProvider.GetRequiredService<IGenericService<Transaction>>();
            this.genericMarketItemService = scope.ServiceProvider.GetRequiredService<IGenericService<MarketItem>>();
            this.genericMarketItemRepository = scope.ServiceProvider.GetRequiredService<IGenericRepository<MarketItem>>();
            this.productItemService = scope.ServiceProvider.GetRequiredService<IProductItemService>();
        }
        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            await Task.Delay(5000, stoppingToken);
            while (!stoppingToken.IsCancellationRequested)
            {
                if (ScannerServiceState.IsScanned)
                {
                    var limitOptionRequests = (await limitOptionRequestGenericRepository.GetAllAsync()).Where(x => x.InProgress && !x.IsDeleted).ToList();
                    var marketItems = (await genericMarketItemRepository.GetAllAsync()).Where(x => x.InProgress && !x.IsDeleted).ToList();
                    foreach (var marketItem in marketItems)
                    {
                        var requests = limitOptionRequests.Where(x => x.ProductId == marketItem.ProductId && x.UnitPrice == marketItem.UnitPrice).OrderBy(x => x.CreatedTime).ToList();
                        foreach (var request in requests)
                        {
                            if (!marketItem.InProgress)
                                break;
                            request.Wallet = await walletGenericRepository.GetByIdAsync(request.WalletId);
                            double satinalinabilecekitemsayisi = request.Wallet.Money / (marketItem.UnitPrice * (1 + ComissionRate));
                            double satinalinacaksayisi = 0;
                            if (request.Amount <= satinalinabilecekitemsayisi)
                            {
                                if (request.Amount >= marketItem.Amount)
                                    satinalinacaksayisi = marketItem.Amount;
                                else
                                    satinalinacaksayisi = request.Amount;
                            }
                            else
                                satinalinacaksayisi = satinalinabilecekitemsayisi;


                            //Limit islemina aktarilan para ve adet sistemi guncellendi
                            request.Money -= satinalinacaksayisi * (marketItem.UnitPrice * (1 + ComissionRate));
                            request.Amount -= satinalinacaksayisi;
                            request.UpdateUserId = Guid.Parse(UserStringInfo.SystemUserId);
                            if (request.Amount == 0)
                                request.InProgress = false;
                            await limitOptionRequestGenericRepository.UpdateAsync(request);
                            await limitOptionRequestGenericRepository.Commit();

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
                            var productItem = await productItemService.GetAsync(request.WalletId, request.ProductId);
                            productItem.Amount += satinalinacaksayisi;
                            productItem.UpdateUserId = Guid.Parse(UserStringInfo.SystemUserId);
                            await genericProductItemRepository.UpdateAsync(productItem);
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
                                ProductId = request.ProductId,
                                UnitPrice = marketItem.UnitPrice,
                                WalletIdBuyer = request.WalletId,
                                WalletIdSeller = sellerWallet.Id
                            };
                            await genericTransactionService.AddAsync(transaction);
                            await genericTransactionService.Commit();

                            genericTransactionService.BeginTransaction();
                            genericMarketItemService.BeginTransaction();
                            genericProductItemRepository.BeginTransaction();
                            genericWalletService.BeginTransaction();
                            limitOptionRequestGenericRepository.BeginTransaction();

                        }
                    }
                    ScannerServiceState.IsScanned = false;
                }
            }
        }
    }
}