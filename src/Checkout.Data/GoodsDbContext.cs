using Checkout.Data.EntityTypeConfigurations;
using Checkout.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Checkout.Data
{
    public class GoodsDbContext : DbContext
    {
        public DbSet<Good> Goods { get; set; }

        public GoodsDbContext(DbContextOptions<GoodsDbContext> options) : base(options)
        { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.ApplyConfiguration(new GoodsConfiguration());
        }
    }
}