using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using Blazored.LocalStorage;

using GeneralStockMarket.ClientShared.Services.Interfaces;
using GeneralStockMarket.ClientShared.Settings;
using GeneralStockMarket.ClientShared.Settings.Interfaces;
using GeneralStockMarket.ClientShared.StringInfo;
using GeneralStockMarket.ClientShared.ViewModels;
using GeneralStockMarket.CoreLib.ExtensionMethods;
using GeneralStockMarket.CoreLib.Response;

using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

namespace GeneralStockMarket.BlazorServerAppClient.Controllers
{
    public class AuthController : Controller
    {
        private readonly IIdentityService identityService;
        private readonly ITokenService tokenService;

        public AuthController(
            IIdentityService identityService,
            ITokenService tokenService)
        {
            this.identityService = identityService;
            this.tokenService = tokenService;
        }

        private async Task logoutAsync()
        {
            await identityService.SignOutAsync();
            await tokenService.RevokeRefreshTokenAsync();
        }

        [HttpGet]
        public async Task<IActionResult> Logout()
        {
            await logoutAsync();
            return Redirect("/auth/login");
        }


        [HttpGet]
        public async Task<IActionResult> Login(string returnUrl = "")
        {
            TempData.Remove("ReturnUrl");
            TempData.Add("ReturnUrl", returnUrl);
            await logoutAsync();
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            if (!ModelState.IsValid)
                return View(model);

            await identityService.SignOutAsync();
            var res = await identityService.SignInAsync(model);

            if (!res.IsSuccessful)
            {
                ViewBag.ErrorMessage = Error.GetError(res.ErrorData, "<br/>");
                return View(model);
            }


            var returnUrl = TempData["ReturnUrl"].ToString();
            if (!returnUrl.IsEmpty())
                return Redirect($"/{returnUrl}");
            return Redirect("/");
        }
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(RegisterViewModel model)
        {
            if (!ModelState.IsValid)
                return View(model);

            var token = await tokenService.ConnectTokenAsync();
            var res = await identityService.SignUpAsync(model, token.Data);

            if (!res.IsSuccessful)
            {
                ViewBag.ErrorMessage = Error.GetError(res.ErrorData, "<br/>");
                return View(model);
            }
            return RedirectToAction("Login");
        }

        [HttpGet]
        public async Task<IActionResult> RefreshToken()
        {
            await identityService.UpdateUserClaimsAsync();
            return Redirect("/account");
        } 
    }
}
