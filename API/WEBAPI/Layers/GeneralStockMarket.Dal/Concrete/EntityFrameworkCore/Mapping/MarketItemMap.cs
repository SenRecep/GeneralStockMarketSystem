using GeneralStockMarket.Dal.Concrete.EntityFrameworkCore.Mapping.ExtensionMethods;
using GeneralStockMarket.Entities.Concrete;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace GeneralStockMarket.Dal.Concrete.EntityFrameworkCore.Mapping
{
    public class MarketItemMap : IEntityTypeConfiguration<MarketItem>
    {
        public void Configure(EntityTypeBuilder<MarketItem> builder)
        {
            builder.EntityBaseMap();
            builder.Property(x => x.UnitPrice).HasDefaultValue(0.0).IsRequired();
            builder.Property(x => x.Amount).HasDefaultValue(0.0).IsRequired();
            builder.Property(x => x.InProgress).HasDefaultValue(false).IsRequired();
        }
    }
}
