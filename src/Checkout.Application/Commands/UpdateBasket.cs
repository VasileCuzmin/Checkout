using Checkout.Domain.BasketAggregate;
using Checkout.Domain.Repositories;
using FluentValidation;
using MediatR;
using NBB.Data.Abstractions;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Checkout.Application.Commands
{
    public class UpdateBasket
    {
        public record Command(Guid GoodId, int Quantity, Guid BasketId) : IRequest;

        public class Validator : AbstractValidator<Command>
        {
            private readonly IGoodsRepository _goodsRepository;

            public Validator(IGoodsRepository stockRepository)
            {
                _goodsRepository = stockRepository;

                RuleFor(a => a.GoodId)
                    .NotEmpty()
                    .MustAsync(ValidateStock).WithMessage("Out of stock!");
            }

            private async Task<bool> ValidateStock(Guid productId, CancellationToken cancellationToken)
            {
                var good = await _goodsRepository.GetGoodById(productId);
                return good.Stock != 0;
            }
        }

        //you could move handlers in application layer
        public class Handler : IRequestHandler<Command>
        {
            private readonly IEventSourcedRepository<Basket> _basketRepository;
            private readonly IGoodsRepository _goodsRepository;

            public Handler(IEventSourcedRepository<Basket> basketRepository, IGoodsRepository goodsRepository)
            {
                _basketRepository = basketRepository;
                _goodsRepository = goodsRepository;
            }

            public async Task<Unit> Handle(Command request, CancellationToken cancellationToken)
            {
                var basket = await _basketRepository.GetByIdAsync(request.BasketId, cancellationToken);
                if (basket == null)
                {
                    throw new ArgumentNullException(nameof(basket));
                    //or create the basket if not exist...but here is supposed to be already created
                }

                var availableProduct = await _goodsRepository.GetGoodById(request.GoodId);
                if (availableProduct == null)
                {
                    throw new ApplicationException($"The product {request.GoodId} doesn't exist!");
                }

                if (request.Quantity > availableProduct.Stock)
                {
                    throw new ApplicationException("The stock is not enough!");
                }

                basket.AddProduct(availableProduct.GoodId, availableProduct.Name, availableProduct.Description,
                                  availableProduct.Price, request.Quantity, availableProduct.Stock);

                await _basketRepository.SaveAsync(basket, cancellationToken);

                return Unit.Value;
            }
        }
    }
}