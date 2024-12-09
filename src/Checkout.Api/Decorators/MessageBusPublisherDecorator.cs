using System.Threading;
using System.Threading.Tasks;
using NBB.Messaging.Abstractions;

namespace CheckoutApi.Decorators
{
    public class MessageBusPublisherDecorator : IMessageBusPublisher
    {
        private readonly IMessageBusPublisher _inner;

        public MessageBusPublisherDecorator(IMessageBusPublisher inner)
        {
            _inner = inner;
        }

        public Task PublishAsync<T>(T message, MessagingPublisherOptions options = null, CancellationToken cancellationToken = default)
        {
            return _inner.PublishAsync(message, MessagingPublisherOptions.Default, cancellationToken);
        }
    }
}