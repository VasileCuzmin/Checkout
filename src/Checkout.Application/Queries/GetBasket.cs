using Checkout.Domain.BasketAggregate;
using MediatR;
using NBB.Data.Abstractions;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Checkout.Application.Queries
{
    public class GetBasket
    {
        public record Query : IRequest<Model>
        {
            public Guid BasketId { get; init; }
        }

        public record Model
        {
            public Guid BasketId { get; set; }
            public Item[] Items { get; set; }
            public decimal TotalNet { get; set; }
            public decimal TotalGross { get; set; }
            public string CustomerName { get; set; }
            public bool PaysVAT { get; set; }
        }

        public class Item
        {
            public string Name { get; set; }
            public decimal Price { get; set; }
        }

        public class QueryHandler : IRequestHandler<Query, Model>
        {
            //for the sake of the homework, let's read the baskets from the event store although this could lead to poor performance.
            //Instead is recommended to have a real model where you can get it.

            private readonly IEventSourcedRepository<Basket> _basketRepository;

            public QueryHandler(IEventSourcedRepository<Basket> basketRepository)
            {
                _basketRepository = basketRepository;
            }

            public async Task<Model> Handle(Query request, CancellationToken cancellationToken)
            {
                var basket = await _basketRepository.GetByIdAsync(request.BasketId, cancellationToken);
                if (basket == null)
                {
                    throw new ArgumentNullException(nameof(basket));
                }

                var model = new Model
                {
                    BasketId = basket.BasketId.Value,
                    CustomerName = basket.User.Name,
                    PaysVAT = basket.User.PaysVAT,
                    Items = basket.Products.Select(p => new Item { Name = p.Name, Price = p.Price.Value }).ToArray(),
                    TotalNet = basket.TotalNet,
                    TotalGross = basket.TotalGross
                };

                return model;
            }
        }
    }
}