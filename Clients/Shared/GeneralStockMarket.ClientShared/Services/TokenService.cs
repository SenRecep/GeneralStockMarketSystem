using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

using AutoMapper;

using GeneralStockMarket.ClientShared.ExtensionMethods;
using GeneralStockMarket.ClientShared.Models;
using GeneralStockMarket.ClientShared.Services.Interfaces;
using GeneralStockMarket.ClientShared.Settings.Interfaces;
using GeneralStockMarket.CoreLib.ExtensionMethods;
using GeneralStockMarket.CoreLib.Response;

using IdentityModel.AspNetCore.AccessTokenManagement;
using IdentityModel.Client;

using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Protocols.OpenIdConnect;

namespace GeneralStockMarket.ClientShared.Services
{
    public class TokenService : ITokenService
    {
        private readonly HttpClient client;
        private readonly IHttpContextAccessor httpContextAccessor;
        private readonly IMapper mapper;
        private readonly ILogger<TokenService> logger;
        private readonly IClientSettings clientSettings;
        private readonly IClientAccessTokenCache clientAccessTokenCache;

        public TokenService(
           HttpClient httpClient,
           IHttpContextAccessor httpContextAccessor,
           IMapper mapper,
           ILogger<TokenService> logger,
           IClientSettings clientSettings,
           IClientAccessTokenCache clientAccessTokenCache)
        {
            client = httpClient;
            this.httpContextAccessor = httpContextAccessor;
            this.mapper = mapper;
            this.logger = logger;
            this.clientSettings = clientSettings;
            this.clientAccessTokenCache = clientAccessTokenCache;
        }

        public async Task<Response<string>> ConnectTokenAsync()
        {
            Response<string> response = null;

            var currentToken = await clientAccessTokenCache.GetAsync("WebClientToken");

            if (currentToken is not null)
            {
                response = Response<string>.Success(currentToken.AccessToken, StatusCodes.Status200OK);
                logger.LogResponse(response);
                return response;
            }

            TokenResponse res = await client.RequestClientCredentialsTokenAsync(new ClientCredentialsTokenRequest()
            {
                Address = "connect/token",
                ClientId = clientSettings.WebClient.ClientId,
                ClientSecret = clientSettings.WebClient.ClientSecret,
                Scope = "IdentityServerApi"
            });

            if (res.IsError)
            {
                response = await res.GetResponseAsync<string>(true, "TokenService/ConnectTokenAsync", "Token alınırken beklenmedik bir hata ile karşılaşıldı");
                logger.LogResponse(response);
                return response;
            }

            await clientAccessTokenCache.SetAsync("WebClientToken", res.AccessToken, res.ExpiresIn);

            response = Response<string>.Success(res.AccessToken, (int)res.HttpStatusCode);
            logger.LogResponse(response);
            return response;
        }
        public async Task<Response<Token>> RefreshTokenAsync()
        {
            var refreshToken = await httpContextAccessor.HttpContext.GetTokenAsync(OpenIdConnectParameterNames.RefreshToken);
            TokenResponse res = await client.RequestRefreshTokenAsync(new RefreshTokenRequest()
            {
                Address = "connect/token",
                ClientId = clientSettings.WebClientForUser.ClientId,
                ClientSecret = clientSettings.WebClientForUser.ClientSecret,
                RefreshToken = refreshToken
            });

            Response<Token> response = null;
            if (res.IsError)
            {
                response = await res.GetResponseAsync<Token>(true, "TokenService/RefreshTokenAsync", "Refresh token alinirken beklenmedik bir hata ile karşılaşıldı");
                logger.LogResponse(response);
                return response;
            }

            Token result = mapper.Map<Token>(res);

            response = Response<Token>.Success(result, (int)res.HttpStatusCode);
            logger.LogResponse(response);
            return response;
        }
        public async Task<Response<string>> RevokeRefreshTokenAsync()
        {
            string refreshToken = await httpContextAccessor.HttpContext.GetTokenAsync(OpenIdConnectParameterNames.RefreshToken);

            if (!refreshToken.IsEmpty())
            {
                TokenRevocationRequest tokenRevocationRequest = new()
                {
                    Address = "/connect/revocation",
                    ClientId = clientSettings.WebClientForUser.ClientId,
                    ClientSecret = clientSettings.WebClientForUser.ClientSecret,
                    Token = refreshToken,
                    TokenTypeHint = clientSettings.GrantType.RefreshTokenCredential
                };

                await client.RevokeTokenAsync(tokenRevocationRequest);
            }
            Response<string> response = Response<string>.Success("Kullanıcıya ait refresh token temizlendi", StatusCodes.Status200OK);
            logger.LogResponse(response);
            return response;
        }
    }
}
