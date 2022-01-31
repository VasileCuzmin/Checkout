using Checkout.Domain.BasketAggregate;
using MediatR;

namespace Checkout.Domain.DomainEvents
{
    public record BasketHasBeenClosed(BasketId BasketId) : INotification;
}