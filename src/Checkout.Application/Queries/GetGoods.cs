using Checkout.Domain.Repositories;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Checkout.Application.Queries
{
    public class GetGoods
    {
        public record Query : IRequest<IEnumerable<Model>>
        {
            //add pagination here if there are hundreds of goods
        }

        public record Model
        {
            public Guid GoodId { get; set; }
            public decimal? Price { get; set; }
            public string Name { get; set; }
            public string Description { get; set; }
            public int Stock { get; set; }
        }

        public class QueryHandler : IRequestHandler<Query, IEnumerable<Model>>
        {
            private readonly IGoodsRepository _goodsRepository;

            public QueryHandler(IGoodsRepository goodsRepository)
            {
                _goodsRepository = goodsRepository;
            }

            public async Task<IEnumerable<Model>> Handle(Query request, CancellationToken cancellationToken)
            {
                var goods = await _goodsRepository.GetAllAsync(cancellationToken);

                return goods.Select(good => new Model
                {
                    GoodId = good.GoodId,
                    Name = good.Name,
                    Description = good.Description,
                    Stock = good.Stock,
                    Price = good.Price
                });
            }
        }
    }
}