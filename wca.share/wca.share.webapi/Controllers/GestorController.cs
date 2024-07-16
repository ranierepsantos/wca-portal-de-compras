using MediatR;
using Microsoft.AspNetCore.Mvc;
using wca.share.application.Features.Gestors.Commands;
using wca.share.application.Features.Gestors.Queries;

namespace wca.share.webapi.Controllers
{
    public class GestorController : ApiController
    {
        private readonly IMediator _mediator;

        public GestorController(IMediator mediator)
        {
            _mediator = mediator;
        }


        [HttpGet("Paginar")]
        public async Task<IActionResult> Paginar([FromQuery] GestorToPaginateQuery query)
        {
            var result = await _mediator.Send(query);

            if (result.IsError) { return Problem(result.Errors); }

            return Ok(result.Value);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] GestorCreateCommand command)
        {
            var result = await _mediator.Send(command);

            if (result.IsError) { return Problem(result.Errors); }

            return Ok(result.Value);
        }

        [HttpPut]
        public async Task<IActionResult> Update([FromBody] GestorUpdateCommand command)
        {
            var result = await _mediator.Send(command);

            if (result.IsError) { return Problem(result.Errors); }

            return Ok(result.Value);
        }

        [HttpGet]
        public async Task<IActionResult> GetById([FromQuery] GestorByIdQuery query)
        {
            var result = await _mediator.Send(query);

            if (result.IsError) { return Problem(result.Errors); }

            return Ok(result.Value);
        }

        [HttpGet("ToComboList")]
        public async Task<IActionResult> ListarGestors()
        {
            var result = await _mediator.Send(new GestorListQuery());

            if (result.IsError) { return Problem(result.Errors); }

            return Ok(result.Value);

        }
    }
}
