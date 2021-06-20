using GeneralStockMarket.Entities.Concrete;

using Microsoft.EntityFrameworkCore;

namespace GeneralStockMarket.Dal.Concrete.EntityFrameworkCore.Contexts
{
    public class GeneralStockMarketDbContext : DbContext
    {
        public GeneralStockMarketDbContext(DbContextOptions<GeneralStockMarketDbContext> options) : base(options)
        {

        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new Mapping.DepositRequestMap());
            modelBuilder.ApplyConfiguration(new Mapping.MarketItemMap());
            modelBuilder.ApplyConfiguration(new Mapping.NewTypeRequestMap());
            modelBuilder.ApplyConfiguration(new Mapping.ProductDepositRequestMap());
            modelBuilder.ApplyConfiguration(new Mapping.ProductItemMap());
            modelBuilder.ApplyConfiguration(new Mapping.ProductMap());
            modelBuilder.ApplyConfiguration(new Mapping.TransactionMap());
            modelBuilder.ApplyConfiguration(new Mapping.WalletMap());
            base.OnModelCreating(modelBuilder);
        }

        public DbSet<LimitOptionRequest> LimitOptionRequests { get; set; }
        public DbSet<DepositRequest> DepositRequests { get; set; }
        public DbSet<MarketItem> MarketItems { get; set; }
        public DbSet<NewTypeRequest> NewTypeRequests { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<ProductDepositRequest> ProductDepositRequests { get; set; }
        public DbSet<ProductItem> ProductItems { get; set; }
        public DbSet<Transaction> Transactions { get; set; }
        public DbSet<Wallet> Wallets { get; set; }
    }
}
