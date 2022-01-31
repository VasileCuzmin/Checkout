using MediatR;
using System;

namespace Checkout.Domain.DomainEvents
{
    public record NewProductHasBeenAdded(Guid ProductId, string Name, decimal? Price, int? Quantity, string Description) : INotification;
}