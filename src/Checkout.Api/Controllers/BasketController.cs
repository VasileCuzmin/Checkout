using Checkout.Api.Models;
using Checkout.Application.Commands;
using Checkout.Application.Queries;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using NBB.Correlation;
using System;
using System.Threading.Tasks;

namespace Checkout.Api.Controllers
{
    [Route("api/baskets")]
    [ApiController]
    public class BasketController : ControllerBase
    {
        private readonly IMediator _mediator;

        public BasketController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet("{basketId}")]
        public async Task<IActionResult> Get([FromRoute] GetBasket.Query query)
        {
            var result = await _mediator.Send(query);
            return Ok(result);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateBasket.Command command)
        {
            await _mediator.Send(command);
            return Ok(new AsyncCommandResult(CorrelationManager.GetCorrelationId()));
        }

        [HttpPut("{basketId}/article-line")]
        public async Task<IActionResult> Update([FromBody] UpdateBasket.Command command, [FromRoute] Guid basketId)
        {
            command = command with { BasketId = basketId };
            await _mediator.Send(command);
            return Ok(new AsyncCommandResult(CorrelationManager.GetCorrelationId()));
        }

        [HttpPatch("{basketId}")]
        public async Task<IActionResult> Close([FromBody] CloseBasket.Command command, [FromRoute] Guid basketId)
        {
            command = command with { BasketId = basketId };
            await _mediator.Send(command);
            return Ok(new AsyncCommandResult(CorrelationManager.GetCorrelationId()));
        }
    }
}