using Checkout.Application.Commands;
using Checkout.Data;
using FluentValidation;
using Hellang.Middleware.ProblemDetails;
using MediatR;
using MediatR.Pipeline;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using NBB.Correlation.AspNet;
using NBB.Domain;
using NBB.Messaging.Abstractions;
using System;
using CheckoutApi;
using CheckoutApi.Decorators;
using CheckoutApi.Swagger;

namespace CheckoutApi
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMvc(options => options.EnableEndpointRouting = false).SetCompatibilityVersion(CompatibilityVersion.Version_3_0);

            services.AddScopedContravariant<IRequestHandler<IRequest, Unit>, MessageBusPublisherCommandHandler>(typeof(CreateBasket).Assembly);

            services.AddMediatR(new[] { typeof(CreateBasket).Assembly });

            services.AddCheckoutDataAccess();

            services.AddEventStore()
                .WithNewtownsoftJsonEventStoreSeserializer(new[] { new SingleValueObjectConverter() })
                .WithAdoNetEventRepository();

            services.TryAddSingleton<IHttpContextAccessor, HttpContextAccessor>();

            services.AddScoped(typeof(IPipelineBehavior<,>), typeof(RequestPreProcessorBehavior<,>));

            services.Scan(scan => scan.FromAssemblyOf<CreateBasket>()
              .AddClasses(classes => classes.AssignableTo<IValidator>())
              .AsImplementedInterfaces()
              .WithScopedLifetime());

            services.AddSwagger();

            services.AddMessageBus().AddNatsTransport(Configuration).UseTopicResolutionBackwardCompatibility(Configuration);
            services.Decorate<IMessageBusPublisher, MessageBusPublisherDecorator>();
            services.AddProblemDetails(ConfigureProblemDetails);
        }

        private void ConfigureProblemDetails(ProblemDetailsOptions options)
        {
            options.IncludeExceptionDetails = (_context, _exception) => true;
            options.MapStatusCode = context => new StatusCodeProblemDetails(context.Response.StatusCode);

            // This will map NotImplementedException to the 501 Not Implemented status code.
            options.Map<NotImplementedException>(_ex => new StatusCodeProblemDetails(StatusCodes.Status501NotImplemented));

            options.Map<ValidationException>(_ex =>
                new StatusCodeProblemDetails(StatusCodes.Status422UnprocessableEntity));
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            app.UseRouting();

            app.UseCors(cors =>
            {
                cors
                    .AllowAnyHeader()
                    .AllowAnyMethod()
                    .AllowAnyOrigin();
            });
            app.UseProblemDetails();
            app.UseCorrelation();

            app.UseEndpoints(builder =>
            {
                builder.MapControllers();
            });

            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "Checkout Api");
            });
        }
    }
}