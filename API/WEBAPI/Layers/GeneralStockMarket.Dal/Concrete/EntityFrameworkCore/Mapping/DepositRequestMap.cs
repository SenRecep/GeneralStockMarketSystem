using GeneralStockMarket.Dal.Concrete.EntityFrameworkCore.Mapping.ExtensionMethods;
using GeneralStockMarket.Entities.Concrete;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace GeneralStockMarket.Dal.Concrete.EntityFrameworkCore.Mapping
{
    public class DepositRequestMap : IEntityTypeConfiguration<DepositRequest>
    {
        public void Configure(EntityTypeBuilder<DepositRequest> builder)
        {
            builder.EntityBaseMap();
            builder.Property(x => x.Description).HasMaxLength(250).IsRequired();
            builder.Property(x => x.Amount).HasDefaultValue(0.0).IsRequired();
            builder.Property(x => x.UserId).IsRequired();
            builder.Property(x => x.Verify).HasDefaultValue(null).IsRequired(false);
        }
    }
}
