using MediatR;

namespace Checkout.Application.Events
{
    public record CheckoutCommandExecutionError : INotification
    {
        public string Code { get; init; }
        public object Data { get; init; }
    }
}