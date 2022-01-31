using Checkout.Application.Events;
using NBB.Core.Pipeline;
using NBB.Domain;
using NBB.Messaging.Abstractions;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Checkout.Worker.MessageMiddlewares
{
    public class HandleExecutionErrorMiddleware : IPipelineMiddleware<MessagingContext>
    {
        private readonly IMessageBusPublisher _messageBusPublisher;

        public HandleExecutionErrorMiddleware(IMessageBusPublisher messageBusPublisher)
        {
            _messageBusPublisher = messageBusPublisher;
        }

        public async Task Invoke(MessagingContext context, CancellationToken cancellationToken, Func<Task> next)
        {
            try
            {
                await next();
            }
            catch (DomainException domainException)
            {
                await _messageBusPublisher.PublishAsync(
                    new CheckoutCommandExecutionError { Code = domainException.Message, Data = context.MessagingEnvelope },
                    cancellationToken);
                
                throw;
            }
            catch (Exception ex)
            {
                await _messageBusPublisher.PublishAsync(
                    new CheckoutCommandExecutionError { Code = ex.Message, Data = context.MessagingEnvelope },
                    cancellationToken);
                
                throw;
            }
        }
    }
}