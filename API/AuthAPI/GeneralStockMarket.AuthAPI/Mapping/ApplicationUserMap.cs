using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using GeneralStockMarket.AuthAPI.Models;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace GeneralStockMarket.AuthAPI.Mapping
{
    public class ApplicationUserMap : IEntityTypeConfiguration<ApplicationUser>
    {
        public void Configure(EntityTypeBuilder<ApplicationUser> builder)
        {
            builder.Property(x=>x.FirstName).HasMaxLength(40).IsRequired(false);
            builder.Property(x=>x.LastName).HasMaxLength(40).IsRequired(false);
            builder.Property(x=>x.IdentityNumber).HasMaxLength(11).IsRequired(false);
            builder.Property(x=>x.Address).HasMaxLength(200).IsRequired(false);
        }
    }
}
