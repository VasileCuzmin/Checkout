using System.Threading;
using System.Threading.Tasks;
using MediatR;
using NBB.Messaging.Abstractions;

namespace CheckoutApi
{
    public class MessageBusPublisherCommandHandler : IRequestHandler<IRequest>
    {
        private readonly IMessageBusPublisher _messageBusPublisher;

        public MessageBusPublisherCommandHandler(IMessageBusPublisher messageBusPublisher)
        {
            _messageBusPublisher = messageBusPublisher;
        }

        public async Task<Unit> Handle(IRequest request, CancellationToken cancellationToken)
        {
            await _messageBusPublisher.PublishAsync(request, cancellationToken);

            return Unit.Value;
        }
    }
}