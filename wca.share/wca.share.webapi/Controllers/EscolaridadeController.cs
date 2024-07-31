using MediatR;
using Microsoft.AspNetCore.Mvc;
using wca.share.application.Features.Escolaridades.Commands;
using wca.share.application.Features.Escolaridades.Queries;

namespace wca.share.webapi.Controllers
{
    public class EscolaridadeController : ApiController
    {
        private readonly IMediator _mediator;

        public EscolaridadeController(IMediator mediator)
        {
            _mediator = mediator;
        }


        [HttpGet("Paginar")]
        public async Task<IActionResult> Paginar([FromQuery] EscolaridadeToPaginateQuery query)
        {
            var result = await _mediator.Send(query);

            if (result.IsError) { return Problem(result.Errors); }

            return Ok(result.Value);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] EscolaridadeCreateCommand command)
        {
            var result = await _mediator.Send(command);

            if (result.IsError) { return Problem(result.Errors); }

            return Ok(result.Value);
        }

        [HttpPut]
        public async Task<IActionResult> Update([FromBody] EscolaridadeUpdateCommand command)
        {
            var result = await _mediator.Send(command);

            if (result.IsError) { return Problem(result.Errors); }

            return Ok(result.Value);
        }

        [HttpGet]
        public async Task<IActionResult> GetById([FromQuery] EscolaridadeByIdQuery query)
        {
            var result = await _mediator.Send(query);

            if (result.IsError) { return Problem(result.Errors); }

            return Ok(result.Value);
        }

        [HttpGet("ToComboList")]
        public async Task<IActionResult> ListarEscolaridades()
        {
            var result = await _mediator.Send(new EscolaridadeListQuery());

            if (result.IsError) { return Problem(result.Errors); }

            return Ok(result.Value);

        }
    }
}
