using Checkout.Application.Commands;
using Checkout.Data;
using Checkout.Data.Repositories;
using Checkout.Domain.Repositories;
using Checkout.Worker.MessageMiddlewares;
using MediatR;
using MediatR.Pipeline;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using NBB.Core.Pipeline;
using NBB.Domain;
using NBB.Messaging.Abstractions;
using NBB.Messaging.Host;

namespace Checkout.Worker
{
    public static class DependencyInjectionExtensions
    {
        public static void AddWorkerServices(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddCheckoutDataAccess();

            services.AddEventStore()
                .WithNewtownsoftJsonEventStoreSeserializer(new[] { new SingleValueObjectConverter() })
                .WithAdoNetEventRepository();

            // MediatR & Messaging
            services.AddMediatR(typeof(CreateBasket.Command).Assembly);
            //services.AddScopedContravariant<INotificationHandler<INotification>, MessageBusPublisherEventHandler>(typeof(ProcessUpdated).Assembly);

            services.AddScoped(typeof(IPipelineBehavior<,>), typeof(RequestPreProcessorBehavior<,>));
            services.AddMessageBus().AddNatsTransport(configuration).UseTopicResolutionBackwardCompatibility(configuration);

            services
                .Decorate<IMessageBusPublisher, MessageBusPublisherDecorator>();

            services.AddMessagingHost(
                hostBuilder => hostBuilder
                .Configure(configBuilder =>
                {
                    static IPipelineBuilder<MessagingContext> basePipeline(IPipelineBuilder<MessagingContext> builder) => builder
                        .UseCorrelationMiddleware()
                        .UseExceptionHandlingMiddleware()
                        .UseMiddleware<HandleExecutionErrorMiddleware>()
                        .UseDefaultResiliencyMiddleware();

                    configBuilder
                        .AddSubscriberServices(config => config
                             .FromMediatRHandledCommands().AddClassesWhere(x => x.Assembly == typeof(CreateBasket.Command).Assembly))
                        .WithDefaultOptions()
                        .UsePipeline(builder => basePipeline(builder)
                            .UseMediatRMiddleware());
                }));
        }
    }
}