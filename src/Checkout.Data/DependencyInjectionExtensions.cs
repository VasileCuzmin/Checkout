using Checkout.Data.Repositories;
using Checkout.Domain.BasketAggregate;
using Checkout.Domain.Entities;
using Checkout.Domain.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using NBB.Core.Abstractions;
using NBB.Data.EntityFramework;

namespace Checkout.Data
{
    public static class DependencyInjectionExtensions
    {
        public static void AddCheckoutDataAccess(this IServiceCollection services)
        {
            services.AddEntityFrameworkDataAccess();
            services.AddEventSourcingDataAccess((sp, builder) =>
                         builder.Options.DefaultSnapshotVersionFrequency = 10)
                    .AddEventSourcedRepository<Basket>();

            services
                    .AddDbContextPool<GoodsDbContext>(
                        (serviceProvider, options) =>
                        {
                            var configuration = serviceProvider.GetService<IConfiguration>();
                            var connectionString = configuration.GetConnectionString("Checkout_Database");

                            options
                                .UseSqlServer(connectionString, builder => { builder.EnableRetryOnFailure(3); })
                                .UseQueryTrackingBehavior(QueryTrackingBehavior.TrackAll);
                        });

            services.AddGoodsRepository();
        }

        private static void AddGoodsRepository(this IServiceCollection services)
        {
            services.AddScoped<IUow<Good>, EfUow<Good, GoodsDbContext>>();
            services.AddScoped<IGoodsRepository, GoodsRepository>();
        }
    }
}