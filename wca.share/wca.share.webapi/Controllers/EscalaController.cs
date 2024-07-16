using MediatR;
using Microsoft.AspNetCore.Mvc;
using wca.share.application.Features.Escalas.Commands;
using wca.share.application.Features.Escalas.Queries;

namespace wca.share.webapi.Controllers
{
    public class EscalaController : ApiController
    {
        private readonly IMediator _mediator;

        public EscalaController(IMediator mediator)
        {
            _mediator = mediator;
        }


        [HttpGet("Paginar")]
        public async Task<IActionResult> Paginar([FromQuery] EscalaToPaginateQuery query)
        {
            var result = await _mediator.Send(query);

            if (result.IsError) { return Problem(result.Errors); }

            return Ok(result.Value);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] EscalaCreateCommand command)
        {
            var result = await _mediator.Send(command);

            if (result.IsError) { return Problem(result.Errors); }

            return Ok(result.Value);
        }

        [HttpPut]
        public async Task<IActionResult> Update([FromBody] EscalaUpdateCommand command)
        {
            var result = await _mediator.Send(command);

            if (result.IsError) { return Problem(result.Errors); }

            return Ok(result.Value);
        }

        [HttpGet]
        public async Task<IActionResult> GetById([FromQuery] EscalaByIdQuery query)
        {
            var result = await _mediator.Send(query);

            if (result.IsError) { return Problem(result.Errors); }

            return Ok(result.Value);
        }

        [HttpGet("ToComboList")]
        public async Task<IActionResult> ListarEscalas()
        {
            var result = await _mediator.Send(new EscalaListQuery());

            if (result.IsError) { return Problem(result.Errors); }

            return Ok(result.Value);

        }
    }
}
