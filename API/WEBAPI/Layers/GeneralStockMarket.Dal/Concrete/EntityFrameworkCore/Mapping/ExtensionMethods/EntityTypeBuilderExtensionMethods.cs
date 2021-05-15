using GeneralStockMarket.Core.Entities.Abstract;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace GeneralStockMarket.Dal.Concrete.EntityFrameworkCore.Mapping.ExtensionMethods
{
    public static class EntityTypeBuilderExtensionMethods
    {
        public static void EntityBaseMap<TEntity>(this EntityTypeBuilder<TEntity> builder)
            where TEntity : class, IEntityBase, new()
        {
            builder.HasKey(x => x.Id);
            builder.Property(x => x.Id).HasDefaultValueSql("newid()").ValueGeneratedOnAdd();
            builder.Property(x => x.CreatedTime).HasDefaultValueSql("getdate()").ValueGeneratedOnAdd().IsRequired();
            builder.Property(x => x.UpdateTime).IsRequired(false);
            builder.Property(x => x.CreatedUserId).IsRequired();
            builder.Property(x => x.UpdateUserId).IsRequired(false);
            builder.Property(x => x.IsDeleted).HasDefaultValue(false).IsRequired();

        }
    }
}
