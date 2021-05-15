using GeneralStockMarket.Dal.Concrete.EntityFrameworkCore.Mapping.ExtensionMethods;
using GeneralStockMarket.Entities.Concrete;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace GeneralStockMarket.Dal.Concrete.EntityFrameworkCore.Mapping
{
    public class TransactionMap : IEntityTypeConfiguration<Transaction>
    {
        public void Configure(EntityTypeBuilder<Transaction> builder)
        {
            builder.EntityBaseMap();
            builder.Property(x => x.UnitPrice).HasDefaultValue(0.0).IsRequired();
            builder.Property(x => x.Amount).HasDefaultValue(0.0).IsRequired();
            builder.HasOne(x => x.WalletBuyer)
                .WithMany(x => x.TransactionBuyer)
                .HasForeignKey(x => x.WalletIdBuyer)
                .OnDelete(DeleteBehavior.NoAction);
            builder.HasOne(x => x.WalletSeller)
                .WithMany(x => x.TransactionSeller)
                .HasForeignKey(x => x.WalletIdSeller)
                .OnDelete(DeleteBehavior.NoAction);
        }
    }
}
