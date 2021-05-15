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

namespace GeneralStockMarket.ClientShared.Interceptors
{
    public class ClientCredentialTokenHandler : DelegatingHandler
    {
        private readonly ITokenService tokenService;

        public ClientCredentialTokenHandler(ITokenService tokenService)
        {
            this.tokenService = tokenService;
        }
        protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            var tokenResponse = await tokenService.ConnectTokenAsync();
            if (tokenResponse.IsSuccessful)
                request.SetBearerToken(tokenResponse.Data);

            var response = await base.SendAsync(request, cancellationToken);

            if (response.StatusCode == System.Net.HttpStatusCode.Unauthorized)
                throw new UnAuthorizeException();
            return response;
        }
    }
}
