using FluentValidation.AspNetCore;

using GeneralStockMarket.ApiShared.ExtensionMethods;
using GeneralStockMarket.ApiShared.Services;
using GeneralStockMarket.ApiShared.Services.Interfaces;
using GeneralStockMarket.Bll.Interfaces;
using GeneralStockMarket.Bll.Managers;
using GeneralStockMarket.Bll.Mapping;
using GeneralStockMarket.Dal.Concrete.EntityFrameworkCore.Contexts;
using GeneralStockMarket.Dal.Concrete.EntityFrameworkCore.Repositories;
using GeneralStockMarket.Dal.Interface;
using GeneralStockMarket.DTO.Validation.ValidationRules.FluentValidation.Product;

using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace GeneralStockMarket.Bll.Containers.MicrosoftIOC
{
    public static class MicrosoftIocExtension
    {

        public static void AddDependencies(this IServiceCollection services, IConfiguration configuration, IWebHostEnvironment environment)
        {
            string connectionString = configuration.GetCustomConnectionString(environment.GetConnectionType());
            string migrationName = "GeneralStockMarket.WebAPI";

            services.AddTransient<DbContext, GeneralStockMarketDbContext>();

            services.AddDbContext<GeneralStockMarketDbContext>(opt =>
                opt.UseSqlServer(connectionString, sqlOpt =>
                    sqlOpt.MigrationsAssembly(migrationName)
                    )
            );

            services.AddHttpContextAccessor();

            services.AddTransient(typeof(IGenericService<>), typeof(GenericManager<>));
            services.AddTransient(typeof(IGenericRepository<>), typeof(EfGenericRepository<>));

            services.AddScoped<IProductService, ProductManager>();
            services.AddScoped<IProductRepository, EfProductRepository>();

            services.AddScoped<IWalletService, WalletManager>();
            services.AddScoped<IWalletRepository, EfWalletRepository>();

            services.AddScoped<IProductDepositRequestService, ProductDepositRequestManager>();
            services.AddScoped<IProductDepositRequestRepository, EfProductDepositRequestRepository>();

            services.AddScoped<IProductItemService, ProductItemManager>();
            services.AddScoped<IProductItemRepository, EfProductItemRepository>();

            services.AddScoped<ITradeService, TradeManager>();
            services.AddScoped<ITradeRepository, EfTradeRepository>();

            services.AddScoped<IRequestService, RequestManager>();


            services.AddScoped<ISharedIdentityService,SharedIdentityService>();

            services.AddAutoMapper(typeof(ProductMappingProfile));
            services.AddScoped<ICustomMapper, CustomMapper>();

        }

        public static void AddValidationDependencies(this IMvcBuilder mvcBuilder)
        {
            mvcBuilder.AddFluentValidation(opt =>
            {
                opt.RegisterValidatorsFromAssemblyContaining<ProductCreateDtoValidator>();
            });
        }
    }
}
