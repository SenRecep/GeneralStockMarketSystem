using GeneralStockMarket.Dal.Concrete.EntityFrameworkCore.Mapping.ExtensionMethods;
using GeneralStockMarket.Entities.Concrete;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace GeneralStockMarket.Dal.Concrete.EntityFrameworkCore.Mapping
{
    public class WalletMap : IEntityTypeConfiguration<Wallet>
    {
        public void Configure(EntityTypeBuilder<Wallet> builder)
        {
            builder.EntityBaseMap();
            builder.Property(x => x.UserId).IsRequired();
            builder.Property(x => x.Money).HasDefaultValue(0.0).IsRequired();
            builder.HasMany(x => x.ProductItems)
                .WithOne(x => x.Wallet)
                .HasForeignKey(x => x.WalletId)
                .OnDelete(DeleteBehavior.Cascade);
            builder.HasMany(x => x.MarketItems)
               .WithOne(x => x.Wallet)
               .HasForeignKey(x => x.WalletId)
               .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
