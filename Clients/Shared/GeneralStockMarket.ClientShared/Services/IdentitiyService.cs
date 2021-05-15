using System;
using System.Collections.Generic;
using System.Globalization;
using System.Net.Http;
using System.Net.Http.Json;
using System.Security.Claims;
using System.Threading.Tasks;

using AutoMapper;

using GeneralStockMarket.ClientShared.ExtensionMethods;
using GeneralStockMarket.ClientShared.Models;
using GeneralStockMarket.ClientShared.Services.Interfaces;
using GeneralStockMarket.ClientShared.Settings.Interfaces;
using GeneralStockMarket.ClientShared.ViewModels;
using GeneralStockMarket.CoreLib.ExtensionMethods;
using GeneralStockMarket.CoreLib.Response;

using IdentityModel.Client;

using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Protocols.OpenIdConnect;

namespace GeneralStockMarket.ClientShared.Services
{
    public class IdentitiyService : IIdentityService
    {
        private readonly HttpClient client;
        private readonly IHttpContextAccessor httpContextAccessor;
        private readonly IMapper mapper;
        private readonly ILogger<IdentitiyService> logger;
        private readonly IClientSettings clientSettings;

        public IdentitiyService(
            HttpClient httpClient,
            IHttpContextAccessor httpContextAccessor,
            IMapper mapper,
            ILogger<IdentitiyService> logger,
            IClientSettings clientSettings)
        {
            client = httpClient;
            this.httpContextAccessor = httpContextAccessor;
            this.mapper = mapper;
            this.logger = logger;
            this.clientSettings = clientSettings;
        }

        public async Task<Response<IEnumerable<Claim>>> GetUserInfoAsync(string access_token)
        {
            UserInfoResponse userinfo = await client.GetUserInfoAsync(new()
            {
                Address = "connect/userinfo",
                Token = access_token
            });

            Response<IEnumerable<Claim>> response = null;
            if (userinfo.IsError)
            {
                response = await userinfo.GetResponseAsync<IEnumerable<Claim>>(true, "IdentityService/GetUserInfoAsync", "Kullanıcı bilgileri çekilirken beklenmedik bir hata ile karşılaşıldı");
                logger.LogResponse(response);
                return response;
            }

            response = Response<IEnumerable<Claim>>.Success(userinfo.Claims, (int)userinfo.HttpStatusCode);
            logger.LogResponse(response);
            return response;
        }

        public async Task<Response<string>> RefreshTokenSignInAsync(Token token)
        {
            List<AuthenticationToken> tokens = new List<AuthenticationToken>()
            {
                new AuthenticationToken{ Name=OpenIdConnectParameterNames.AccessToken,Value= token.AccessToken},
                new AuthenticationToken{ Name=OpenIdConnectParameterNames.RefreshToken,Value= token.RefreshToken},
                new AuthenticationToken{ Name=OpenIdConnectParameterNames.ExpiresIn,
                    Value= DateTime.UtcNow.AddSeconds(token.ExpiresIn).ToString("o", CultureInfo.InvariantCulture)}
            };

            AuthenticateResult authenticationResult = await httpContextAccessor.HttpContext.AuthenticateAsync();

            AuthenticationProperties properties = authenticationResult.Properties;

            properties.StoreTokens(tokens);

            await httpContextAccessor.HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, authenticationResult.Principal, properties);

            Response<string> response = Response<string>.Success("Kullanici yeniden giris islemi islemi basari ile gerceklesti", StatusCodes.Status200OK);
            logger.LogResponse(response);
            return response;
        }

        public async Task<Response<Token>> RequestPasswordTokenAsync(LoginViewModel model)
        {
            TokenResponse tokenResponse = await client.RequestPasswordTokenAsync(new()
            {
                Address = "connect/token",
                ClientId = clientSettings.WebClientForUser.ClientId,
                ClientSecret = clientSettings.WebClientForUser.ClientSecret,
                UserName = model.UserName,
                Password = model.Password
            });
            Response<Token> response = null;
            if (tokenResponse.IsError)
            {
                response = await tokenResponse.GetResponseAsync<Token>(true, "IdentityService/RequestPasswordTokenAsync","Token alınırken beklenmedik bir hata ile karşılaşıldı");
                logger.LogResponse(response);
                return response;
            }

            Token result = mapper.Map<Token>(tokenResponse);

            response = Response<Token>.Success(result, (int)tokenResponse.HttpStatusCode);
            logger.LogResponse(response);

            return response;
        }

        public async Task<Response<string>> SignInAsync(LoginViewModel model)
        {
            Response<Token> tokenResponse = await RequestPasswordTokenAsync(model);
            if (!tokenResponse.IsSuccessful)
                return Response<string>.Fail(tokenResponse);

            Token token = tokenResponse.Data;

            Response<IEnumerable<Claim>> userInfoResponse = await GetUserInfoAsync(token.AccessToken);
            if (!userInfoResponse.IsSuccessful)
                return Response<string>.Fail(userInfoResponse);

            IEnumerable<Claim> userInfos = userInfoResponse.Data;

            ClaimsPrincipal claimsPrincipal = new(new ClaimsIdentity(
                userInfos,
                CookieAuthenticationDefaults.AuthenticationScheme,
                "name",
                "role"
               ));

            AuthenticationProperties authenticationProperties = new();
            authenticationProperties.StoreTokens(new List<AuthenticationToken>()
            {
                new(){ Name=OpenIdConnectParameterNames.AccessToken,Value= token.AccessToken},
                new(){ Name=OpenIdConnectParameterNames.RefreshToken,Value= token.RefreshToken},
                new(){ Name=OpenIdConnectParameterNames.ExpiresIn,
                    Value= DateTime.UtcNow.AddSeconds(token.ExpiresIn).ToString("o", CultureInfo.InvariantCulture)}
            });

            authenticationProperties.IsPersistent = model.RememberMe;
            await httpContextAccessor.HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, claimsPrincipal, authenticationProperties);

            Response<string> response = Response<string>.Success("Kullanici Giris islemi basari ile gerceklesti", StatusCodes.Status200OK);
            logger.LogResponse(response);
            return response;
        }

        public async Task<Response<string>> SignOutAsync()
        {
            await httpContextAccessor.HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            Response<string> response = Response<string>.Success("Kullanici çıkış işlemi gerçekleşti", StatusCodes.Status200OK);
            logger.LogResponse(response);
            return response;
        }

        public async Task<Response<string>> SignUpAsync(RegisterViewModel model, string accessToken)
        {
            client.SetBearerToken(accessToken);
            HttpResponseMessage res = await client.PostAsJsonAsync("api/user/SignUp", model);

            if (!res.IsSuccessStatusCode)
            {
                Response<string> response = await res.GetResponseAsync<string>(false, "IdentityService/SignUpAsync", "Kullanıcı kayıt edilirken beklenmedik bir hata ile karşılaşıldı");
                logger.LogResponse(response);
                return response;
            }
            Response<string> result = Response<string>.Success("Kullanici kayit islemi tamamlandi", StatusCodes.Status200OK);
            logger.LogResponse(result);
            return result;
        }

        public async Task<Response<NoContent>> UpdateUserClaimsAsync()
        {
            var accessToken = await httpContextAccessor.HttpContext.GetTokenAsync(OpenIdConnectParameterNames.AccessToken);
            Response<IEnumerable<Claim>> userInfoResponse = await GetUserInfoAsync(accessToken);
            if (!userInfoResponse.IsSuccessful)
                return Response<NoContent>.Fail(userInfoResponse);

            IEnumerable<Claim> userInfos = userInfoResponse.Data;

            ClaimsPrincipal claimsPrincipal = new(new ClaimsIdentity(
                userInfos,
                CookieAuthenticationDefaults.AuthenticationScheme,
                "name",
                "role"
               ));

            AuthenticateResult authenticationResult = await httpContextAccessor.HttpContext.AuthenticateAsync();

            AuthenticationProperties properties = authenticationResult.Properties;

            await httpContextAccessor.HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, claimsPrincipal, properties);

            return Response<NoContent>.Success(StatusCodes.Status200OK);
        }
    }
}
