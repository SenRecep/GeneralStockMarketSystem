using System;
using System.Threading.Tasks;

using Blazored.LocalStorage;

using GeneralStockMarket.ClientShared.Services.Interfaces;
using GeneralStockMarket.ClientShared.StringInfo;
using GeneralStockMarket.DTO.User;

using Microsoft.AspNetCore.Components;

namespace GeneralStockMarket.BlazorServerAppClient.ComponentBases
{
    public class LoginUserComponentBase : ComponentBase
    {
        [Inject]
        private IUserService userService { get; set; }
        [Inject]
        private ILocalStorageService localStorageService { get; set; }


        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            var user = await localStorageService.GetItemAsync<UserDto>(LocalStorageInfo.LoginUser);

            if (user == null)
            {
                var userResponse = await userService.GetUserAsync();

                if (userResponse.IsSuccessful)
                {
                    await localStorageService.RemoveItemAsync(LocalStorageInfo.LoginUser);
                    await localStorageService.SetItemAsync(LocalStorageInfo.LoginUser, userResponse.Data);
                }
            }
        }
    }
}
