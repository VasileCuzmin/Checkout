using MediatR;
using NBB.Messaging.Abstractions;
using System.Threading;
using System.Threading.Tasks;

namespace Checkout.Worker
{
    public class MessageBusPublisherEventHandler : INotificationHandler<INotification>
    {
        private readonly IMessageBusPublisher _messageBusPublisher;

        public MessageBusPublisherEventHandler(IMessageBusPublisher messageBusPublisher)
        {
            _messageBusPublisher = messageBusPublisher;
        }

        public async Task Handle(INotification message, CancellationToken cancellationToken)
        {
            await _messageBusPublisher.PublishAsync(message, cancellationToken);
        }
    }
}