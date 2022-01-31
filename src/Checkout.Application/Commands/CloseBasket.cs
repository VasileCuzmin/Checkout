using Checkout.Domain.BasketAggregate;
using FluentValidation;
using MediatR;
using NBB.Data.Abstractions;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Checkout.Application.Commands
{
    public class CloseBasket
    {
        public record Command(Guid BasketId) : IRequest;

        public class Validator : AbstractValidator<Command>
        {
            //check if the basket exists -- read from the read model not from the event store 
            // -- see the observation from GetBasket query
        }


        //you could move handlers in application layer
        public class Handler : IRequestHandler<Command>
        {
            private readonly IEventSourcedRepository<Basket> _basketRepository;

            public Handler(IEventSourcedRepository<Basket> basketRepository)
            {
                _basketRepository = basketRepository;
            }

            public async Task<Unit> Handle(Command request, CancellationToken cancellationToken)
            {
                var basket = await _basketRepository.GetByIdAsync(request.BasketId, cancellationToken);
                if (basket == null)
                {
                    throw new ArgumentNullException(nameof(basket));
                }

                basket.Close();
                await _basketRepository.SaveAsync(basket, cancellationToken);
                return Unit.Value;
            }
        }
    }
}