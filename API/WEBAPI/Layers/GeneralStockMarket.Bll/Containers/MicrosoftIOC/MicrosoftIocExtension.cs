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
            services.AddTransient<GeneralStockMarketDbContext>();

            services.AddDbContext<GeneralStockMarketDbContext>(opt =>
                opt.UseSqlServer(connectionString, sqlOpt =>
                    sqlOpt.MigrationsAssembly(migrationName)
                    )
            );

            services.AddHttpContextAccessor();

            services.AddTransient(typeof(IGenericService<>), typeof(GenericManager<>));
            services.AddTransient(typeof(IGenericRepository<>), typeof(EfGenericRepository<>));

            services.AddTransient<IProductService, ProductManager>();
            services.AddTransient<IProductRepository, EfProductRepository>();

            services.AddTransient<IWalletService, WalletManager>();
            services.AddTransient<IWalletRepository, EfWalletRepository>();

            services.AddTransient<IProductDepositRequestService, ProductDepositRequestManager>();
            services.AddTransient<IProductDepositRequestRepository, EfProductDepositRequestRepository>();

            services.AddTransient<IProductItemService, ProductItemManager>();
            services.AddTransient<IProductItemRepository, EfProductItemRepository>();

            services.AddTransient<ITradeService, TradeManager>();
            services.AddTransient<ITradeRepository, EfTradeRepository>();

            services.AddTransient<IRequestService, RequestManager>();

            services.AddTransient<ITransactionRepository, EfTransactionRepository>();
            services.AddTransient<ILimitOptionRequestRepository, EfLimitOptionRequestRepository>();


            services.AddScoped<ISharedIdentityService, SharedIdentityService>();

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
