using MediatR;
using Microsoft.AspNetCore.Mvc;
using wca.share.application.Features.TiposFaturamento.Commands;
using wca.share.application.Features.TiposFaturamento.Queries;

namespace wca.share.webapi.Controllers
{
    public class TipoFaturamentoController : ApiController
    {
        private readonly IMediator _mediator;

        public TipoFaturamentoController(IMediator mediator)
        {
            _mediator = mediator;
        }


        [HttpGet("Paginar")]
        public async Task<IActionResult> Paginar([FromQuery] TipoFaturamentoToPaginateQuery query)
        {
            var result = await _mediator.Send(query);

            if (result.IsError) { return Problem(result.Errors); }

            return Ok(result.Value);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] TipoFaturamentoCreateCommand command)
        {
            var result = await _mediator.Send(command);

            if (result.IsError) { return Problem(result.Errors); }

            return Ok(result.Value);
        }

        [HttpPut]
        public async Task<IActionResult> Update([FromBody] TipoFaturamentoUpdateCommand command)
        {
            var result = await _mediator.Send(command);

            if (result.IsError) { return Problem(result.Errors); }

            return Ok(result.Value);
        }

        [HttpGet]
        public async Task<IActionResult> GetById([FromQuery] TipoFaturamentoByIdQuery query)
        {
            var result = await _mediator.Send(query);

            if (result.IsError) { return Problem(result.Errors); }

            return Ok(result.Value);
        }

        [HttpGet("ToComboList")]
        public async Task<IActionResult> ListarTiposFaturamento()
        {
            var result = await _mediator.Send(new TipoFaturamentoListQuery());

            if (result.IsError) { return Problem(result.Errors); }

            return Ok(result.Value);

        }
    }
}
