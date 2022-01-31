using Checkout.Domain.BasketAggregate;
using MediatR;

namespace Checkout.Domain.DomainEvents
{
    public record BasketCreated(BasketId BasketId, User user) : INotification;
}