using System;
using System.Collections.Generic;

using GeneralStockMarket.AuthAPI.Services;

using IdentityServer4;
using IdentityServer4.Models;

namespace GeneralStockMarket.AuthAPI
{
    public static class Config
    {
        public static IEnumerable<ApiResource> ApiResources => new ApiResource[]
        {
            new ApiResource("resource_webapi")
            {
                Scopes={"webapi_fullpermission"},
                ApiSecrets=new []{ new Secret("webapi_secret".Sha256())}
                },
            new ApiResource(IdentityServerConstants.LocalApi.ScopeName)
        };
        public static IEnumerable<ApiScope> ApiScopes =>
            new ApiScope[]
            {
                new ApiScope("webapi_fullpermission","WEB API ICIN TUM IZINLER"),
                new ApiScope(IdentityServerConstants.LocalApi.ScopeName)
            };
        public static IEnumerable<IdentityResource> IdentityResources =>
            new IdentityResource[]
            {
                new IdentityResources.OpenId(),
                new ProfileWithRoleIdentityResource(),
                new IdentityResources.Email(),
            };
        public static IEnumerable<Client> Clients =>
            new Client[]
            {
                new Client
                {
                    ClientId = "WebClient_CC",
                    ClientName = "Web Client CC",

                    AllowedGrantTypes = GrantTypes.ClientCredentials,
                    ClientSecrets = { new Secret("webclient_client_secret".Sha256()) },

                    AllowedScopes =
                    {
                        IdentityServerConstants.LocalApi.ScopeName,
                        "webapi_fullpermission"
                    }
                },

                new Client
                {
                    ClientId = "WebClient_ROP",
                    ClientName = "Web Client ROP",

                    ClientSecrets = { new Secret("webclient_client_secret".Sha256()) },

                    AllowedGrantTypes = GrantTypes.ResourceOwnerPassword,

                    AllowOfflineAccess = true,
                    AllowedScopes = {
                        IdentityServerConstants.StandardScopes.Email,
                        IdentityServerConstants.StandardScopes.OpenId,
                        IdentityServerConstants.StandardScopes.Profile,
                        IdentityServerConstants.StandardScopes.OfflineAccess,
                        IdentityServerConstants.LocalApi.ScopeName,
                        "webapi_fullpermission",
                        "roles"
                    },
                    AccessTokenLifetime =(int)TimeSpan.FromDays(5).TotalSeconds,
                    RefreshTokenUsage=TokenUsage.ReUse,
                    RefreshTokenExpiration=TokenExpiration.Absolute,
                    AbsoluteRefreshTokenLifetime=(int)TimeSpan.FromDays(20).TotalSeconds
                }
            };
    }
}