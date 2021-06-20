
using GeneralStockMarket.Dal.Concrete.EntityFrameworkCore.Mapping.ExtensionMethods;
using GeneralStockMarket.Entities.Concrete;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace GeneralStockMarket.Dal.Concrete.EntityFrameworkCore.Mapping
{
    public class LimitOptionRequestMapping : IEntityTypeConfiguration<LimitOptionRequest>
    {
        public void Configure(EntityTypeBuilder<LimitOptionRequest> builder)
        {
            builder.EntityBaseMap();
            builder.Property(x => x.Money).HasDefaultValue(0.0).IsRequired();
            builder.Property(x => x.UnitPrice).HasDefaultValue(0.0).IsRequired();
            builder.Property(x => x.Amount).HasDefaultValue(0.0).IsRequired();
            builder.Property(x => x.InProgress).HasDefaultValue(true).IsRequired();
            builder.Property(x => x.WalletId).IsRequired();
            builder.Property(x => x.ProductId).IsRequired();
        }
    }
}
