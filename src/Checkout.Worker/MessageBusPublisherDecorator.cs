using NBB.Messaging.Abstractions;
using System.Threading;
using System.Threading.Tasks;

namespace Checkout.Worker
{
    public class MessageBusPublisherDecorator : IMessageBusPublisher
    {
        private readonly IMessageBusPublisher _innerPublisher;

        public MessageBusPublisherDecorator(IMessageBusPublisher innerPublisher)
        {
            _innerPublisher = innerPublisher;
        }

        public Task PublishAsync<T>(T message, MessagingPublisherOptions options = null, CancellationToken cancellationToken = default)
        {
            return _innerPublisher.PublishAsync(message, MessagingPublisherOptions.Default, cancellationToken);
        }
    }
}