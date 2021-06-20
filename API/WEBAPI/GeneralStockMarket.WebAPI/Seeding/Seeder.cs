using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;

using GeneralStockMarket.ApiShared.ExtensionMethods;
using GeneralStockMarket.Bll.Interfaces;
using GeneralStockMarket.Bll.Managers;
using GeneralStockMarket.Bll.Models;
using GeneralStockMarket.Bll.StringInfos;
using GeneralStockMarket.CoreLib.Response;
using GeneralStockMarket.Dal.Concrete.EntityFrameworkCore.Contexts;
using GeneralStockMarket.DTO.User;
using GeneralStockMarket.DTO.Wallet;
using GeneralStockMarket.Entities.Concrete;

using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace GeneralStockMarket.WebAPI.Seeding
{
    public static class Seeder
    {
        public static async Task CreateAccountingWallet(IServiceProvider serviceProvider)
        {
            var httpClient = serviceProvider.GetRequiredService<HttpClient>();
            var enviorment = serviceProvider.GetRequiredService<IWebHostEnvironment>();
            var configuration = serviceProvider.GetRequiredService<IConfiguration>();
            var walletGenericService = serviceProvider.GetRequiredService<IGenericService<Wallet>>();
            var walletService = serviceProvider.GetRequiredService<IWalletService>();
            var identityServerUrl = enviorment.GetIdentityServerUrl(configuration);
            httpClient.BaseAddress = new Uri(identityServerUrl);
            var accountingUserResponse = await httpClient.GetFromJsonAsync<Response<Guid>>("api/user/GetAccountingUser");
            if (accountingUserResponse.IsSuccessful)
            {
                var isExist = await walletService.WalletIsExistByUserIdAsync(accountingUserResponse.Data);
                if (!isExist)
                {
                    WalletCreateDto createWallet = new()
                    {
                        CreatedUserId = Guid.Parse(UserStringInfo.SystemUserId),
                        UserId = accountingUserResponse.Data
                    };

                    var wallet = await walletGenericService.AddAsync(createWallet);
                    await walletGenericService.Commit();
                    if (wallet is null)
                        throw new Exception("Accounting User Wallet Create Error");
                }
                var accountingWallet = await walletService.GetWalletByUserIdAsync(accountingUserResponse.Data);
                AccountingState.AccountingWalletId = accountingWallet.Id;
            }
        }
    }
}
