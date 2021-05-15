using GeneralStockMarket.Dal.Concrete.EntityFrameworkCore.Mapping.ExtensionMethods;
using GeneralStockMarket.Entities.Concrete;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace GeneralStockMarket.Dal.Concrete.EntityFrameworkCore.Mapping
{
    public class ProductMap : IEntityTypeConfiguration<Product>
    {
        public void Configure(EntityTypeBuilder<Product> builder)
        {
            builder.EntityBaseMap();
            builder.Property(x => x.Name).HasMaxLength(100).IsRequired();
            builder.Property(x => x.ImageName).IsRequired();
            builder.HasMany(x => x.ProductItems)
                .WithOne(x => x.Product)
                .HasForeignKey(x => x.ProductId)
                .OnDelete(DeleteBehavior.Cascade);
            builder.HasMany(x => x.MarketItems)
               .WithOne(x => x.Product)
               .HasForeignKey(x => x.ProductId)
               .OnDelete(DeleteBehavior.Cascade);
            builder.HasMany(x => x.Transactions)
               .WithOne(x => x.Product)
               .HasForeignKey(x => x.ProductId)
               .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
