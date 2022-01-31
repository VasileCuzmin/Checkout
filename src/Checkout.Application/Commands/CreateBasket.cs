using Checkout.Domain.BasketAggregate;
using FluentValidation;
using MediatR;
using NBB.Data.Abstractions;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Checkout.Application.Commands
{
    public class CreateBasket
    {
        public record Command(string CustomerName, bool PaysVAT) : IRequest;

        public class Validator : AbstractValidator<Command>
        {
            public Validator()
            {
                RuleFor(a => a.CustomerName)
                    .NotEmpty()
                    .WithMessage("Customer name required!"); ;
            }
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
                var basket = Basket.New(new BasketId(Guid.NewGuid()), new User(request.CustomerName, request.PaysVAT));
                await _basketRepository.SaveAsync(basket, cancellationToken);
                return Unit.Value;
            }
        }
    }
}