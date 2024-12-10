using Checkout.Domain.BasketAggregate;
using MediatR;
using NBB.Data.Abstractions;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Checkout.Application.Queries
{
    public class GetBasketByCustomerName
    {
        public record Query : IRequest<Model>
        {
            public string CustomerName { get; init; }
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
            //private readonly IEventSourcedRepository<Basket> _basketRepository;

            //public QueryHandler(IEventSourcedRepository<Basket> basketRepository)
            //{
            //    _basketRepository = basketRepository;
            //}

            public async Task<Model> Handle(Query request, CancellationToken cancellationToken)
            {
                //var basket = await _basketRepository.GetByIdAsync(request.BasketId, cancellationToken);
                //if (basket == null)
                //{
                //    throw new ArgumentNullException(nameof(basket));
                //}

                //get the basket from the non-event sourced repository
                //mandatory - the user must have only one open basket at the moment

                //var model = new Model
                //{
                //    BasketId = basket.BasketId.Value,
                //    CustomerName = basket.User.Name,
                //    PaysVAT = basket.User.PaysVAT,
                //    Items = basket.Products.Select(p => new Item { Name = p.Name, Price = p.Price.Value }).ToArray(),
                //    TotalNet = basket.TotalNet,
                //    TotalGross = basket.TotalGross
                //};

                throw new NotImplementedException();
            }
        }
    }
}
