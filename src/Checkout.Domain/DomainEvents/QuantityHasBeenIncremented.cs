using MediatR;
using System;

namespace Checkout.Domain.DomainEvents
{
    public record QuantityHasBeenIncremented(Guid ProductId, int AddedQuantity, int? LastQuantity, decimal? Price) : INotification;
}