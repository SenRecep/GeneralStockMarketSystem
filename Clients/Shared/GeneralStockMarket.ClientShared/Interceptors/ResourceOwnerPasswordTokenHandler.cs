using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

using GeneralStockMarket.ClientShared.Exceptions;
using GeneralStockMarket.ClientShared.Services.Interfaces;

using IdentityModel.Client;

using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Protocols.OpenIdConnect;

namespace GeneralStockMarket.ClientShared.Interceptors
{
    public class ResourceOwnerPasswordTokenHandler : DelegatingHandler
    {
        private readonly IHttpContextAccessor httpContextAccessor;
        private readonly IIdentityService identityService;
        private readonly ITokenService tokenService;

        public ResourceOwnerPasswordTokenHandler(
            IHttpContextAccessor httpContextAccessor,
            IIdentityService identityService,
            ITokenService tokenService)
        {
            this.httpContextAccessor = httpContextAccessor;
            this.identityService = identityService;
            this.tokenService = tokenService;
        }

        protected async override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            var accessToken = await httpContextAccessor.HttpContext.GetTokenAsync(OpenIdConnectParameterNames.AccessToken);

            request.SetBearerToken(accessToken);

            var response = await base.SendAsync(request, cancellationToken);

            if (response.StatusCode == System.Net.HttpStatusCode.Unauthorized)
            {
                var tokenResponse = await tokenService.RefreshTokenAsync();

                if (tokenResponse.IsSuccessful)
                {
                    await identityService.RefreshTokenSignInAsync(tokenResponse.Data);

                    request.SetBearerToken(tokenResponse.Data.AccessToken);

                    response = await base.SendAsync(request, cancellationToken);
                }
            }

            if (response.StatusCode == System.Net.HttpStatusCode.Unauthorized)
                throw new UnAuthorizeException();

            return response;
        }
    }
}
