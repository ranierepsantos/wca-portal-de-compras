using MediatR;
using Microsoft.AspNetCore.Mvc;
using wca.reembolso.application.Features.Faturamentos.Commands;
using wca.reembolso.application.Features.Faturamentos.Queries;
using wca.reembolso.application.Features.Solicitacaos.Queries;
using wca.reembolso.application.Features.Solicitacoes.Commands;

namespace wca.reembolso.webapi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FaturamentoController : ApiController
    {
        private readonly IMediator _mediator;

        public FaturamentoController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet]
        public async Task<IActionResult> GetById([FromQuery] FaturamentoByIdQuery query) 
        {
            var result = await _mediator.Send(query);
        
            if (result.IsError) { return Problem(result.Errors); }

            return Ok(result.Value);
        }

        [HttpGet("Paginar")]
        public async Task<IActionResult> ToPaginate([FromQuery] FaturamentoPaginateQuery query)
        {
            var result = await _mediator.Send(query);

            if (result.IsError) { return Problem(result.Errors); }

            return Ok(result.Value);
        }

        [HttpPost]
        public async Task<IActionResult> Create(FaturamentoCreateCommand command)
        {
            var result = await _mediator.Send(command);

            if (result.IsError) { return Problem(result.Errors); }

            return Ok(result.Value);
        }

        [HttpPost("AdicionarPO")]
        public async Task<IActionResult> AddPO(FaturamentoAddPOCommand command)
        {
            var result = await _mediator.Send(command);

            if (result.IsError) { return Problem(result.Errors); }

            return Ok(result.Value);
        }

        [HttpPost("Finalizar")]
        public async Task<IActionResult> Finalizar(FaturamentoFinalizarCommand command)
        {
            var result = await _mediator.Send(command);

            if (result.IsError) { return Problem(result.Errors); }

            return Ok(result.Value);
        }

        [HttpPut("AlterarStatus")]
        public async Task<IActionResult> AlterarStatus([FromBody] FaturamentoChangeStatusCommand command)
        {
            var result = await _mediator.Send(command);

            if (result.IsError) { return Problem(result.Errors); }

            return Ok(result.Value);

        }
    }
}
