using Checkout.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Checkout.Data.EntityTypeConfigurations
{
    internal class GoodsConfiguration : IEntityTypeConfiguration<Good>
    {
        public void Configure(EntityTypeBuilder<Good> builder)
        {
            builder.ToTable("Goods").HasKey(x => new { x.GoodId });
        }
    }
}