using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using FluentValidation;
using FluentValidation.AspNetCore;

using GeneralStockMarket.ClientShared.AutoMapperProfile.Mapping;
using GeneralStockMarket.ClientShared.ExtensionMethods;
using GeneralStockMarket.ClientShared.Helpers;
using GeneralStockMarket.ClientShared.Interceptors;
using GeneralStockMarket.ClientShared.Services;
using GeneralStockMarket.ClientShared.Services.Interfaces;
using GeneralStockMarket.ClientShared.Settings;
using GeneralStockMarket.ClientShared.Settings.Interfaces;
using GeneralStockMarket.ClientShared.ViewModels;
using GeneralStockMarket.DTO.Validation.ValidationRules.FluentValidation.User;

using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

namespace GeneralStockMarket.ClientShared.Containers.MicrosoftIOC
{
    public static class MicrosoftIocExtension
    {
        public static void AddDependencies(this IServiceCollection services, IConfiguration configuration, IWebHostEnvironment environment)
        {
            services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
                .AddCookie(CookieAuthenticationDefaults.AuthenticationScheme, opts =>
            {
                opts.LoginPath = "/Auth/Login";
                opts.AccessDeniedPath = "/Auth/AccessDenied";
                opts.ExpireTimeSpan = TimeSpan.FromDays(20 + 1);
                opts.SlidingExpiration = true;
            });

            services.AddHttpContextAccessor();
            services.AddAccessTokenManagement();


            services.AddSingleton<PhotoHelper>();

            #region Settings
            services.Configure<ClientSettings>(configuration.GetSection("ClientSettings"));
            services.AddSingleton<IClientSettings>(sp => sp.GetRequiredService<IOptions<ClientSettings>>().Value);

            services.Configure<ServicesBaseUrlSettings>(configuration.GetSection("ServicesBaseUrls"));
            services.AddSingleton<IServicesBaseUrlSettings>(sp => sp.GetRequiredService<IOptions<ServicesBaseUrlSettings>>().Value);
            #endregion

            var serviceApiSettings = configuration.GetSection("ServicesBaseUrls").Get<ServicesBaseUrlSettings>();
            var currentApiSettings = serviceApiSettings.GetServiceUrlsByEnvironment(environment);


            services.AddScoped<ResourceOwnerPasswordTokenHandler>();
            services.AddScoped<ClientCredentialTokenHandler>();

            services.AddHttpClient();
            services.AddHttpClient<IIdentityService, IdentitiyService>(opt => opt.BaseAddress = new(currentApiSettings.IdentityServer));
            services.AddHttpClient<ITokenService, TokenService>(opt => opt.BaseAddress = new(currentApiSettings.IdentityServer));
            services.AddHttpClient<IUserService, UserService>(opt => opt.BaseAddress = new(currentApiSettings.IdentityServer))
                .AddHttpMessageHandler<ResourceOwnerPasswordTokenHandler>();

            services.AddHttpClient<IProductService, ProductService>(opt => opt.BaseAddress = new(currentApiSettings.WebApi))
                .AddHttpMessageHandler<ResourceOwnerPasswordTokenHandler>();

            services.AddHttpClient<IWalletService, WalletService>(opt => opt.BaseAddress = new(currentApiSettings.WebApi))
              .AddHttpMessageHandler<ResourceOwnerPasswordTokenHandler>();

            services.AddHttpClient<IRequestService, RequestService>(opt => opt.BaseAddress = new(currentApiSettings.WebApi))
              .AddHttpMessageHandler<ResourceOwnerPasswordTokenHandler>();

            services.AddHttpClient<ITradeService, TradeService>(opt => opt.BaseAddress = new(currentApiSettings.WebApi))
              .AddHttpMessageHandler<ResourceOwnerPasswordTokenHandler>();

            services.AddAutoMapper(typeof(IdentityResponseGeneralProfile));

            services.AddValidatorsFromAssemblyContaining<UpdateUserValidator>();
        }

        public static void AddValidationDependencies(this IMvcBuilder mvcBuilder)
        {
            mvcBuilder.AddFluentValidation(opt =>
            {
                opt.RegisterValidatorsFromAssemblyContaining<LoginViewModel>();
            });
        }
    }
}
