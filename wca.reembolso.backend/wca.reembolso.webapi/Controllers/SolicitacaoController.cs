using MediatR;
using Microsoft.AspNetCore.Mvc;
using wca.reembolso.application.Features.Solicitacaos.Queries;
using wca.reembolso.application.Features.Solicitacoes.Commands;
using wca.reembolso.application.Features.Solicitacoes.Queries;

namespace wca.reembolso.webapi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    
    public class SolicitacaoController : ApiController
    {
        private readonly IMediator _mediator;

        public SolicitacaoController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet]
        public async Task<IActionResult> GetById([FromQuery] SolicitacaoByIdQuerie command) 
        {
            var result = await _mediator.Send(command);
        
            if (result.IsError) { return Problem(result.Errors); }

            return Ok(result.Value);
        }

        [HttpPost]
        public async Task<IActionResult> Create(SolicitacaoCreateCommand createCommand)
        {
            var result = await _mediator.Send(createCommand);

            if (result.IsError) { return Problem(result.Errors); }

            return Ok(result.Value);
        }

        [HttpGet("Paginar")]
        public async Task<IActionResult> ToPaginate([FromQuery] SolicitacaoPaginateQuery querie)
        {
            var result = await _mediator.Send(querie);

            if (result.IsError) { return Problem(result.Errors); }

            return Ok(result.Value);
        }

        [HttpPut]
        public async Task<IActionResult> Update(SolicitacaoUpdateCommand updateCommand)
        {
            var result = await _mediator.Send(updateCommand);

            if (result.IsError) { return Problem(result.Errors); }

            return Ok(result.Value);
        }

        [HttpGet("ListarPorColaboradorGestor")]
        public async Task<IActionResult> ListarPorColaboradorGestor([FromQuery] SolicitacaoByColaboradorOrGestorQuerie querie)
        {
            var result = await _mediator.Send(querie);

            if (result.IsError) { return Problem(result.Errors); }

            return Ok(result.Value);
        }

        [HttpGet("ListarStatusSolicitacao")]
        public async Task<IActionResult> ListarStatusSolicitacao()
        {
            var result = await _mediator.Send(new StatusSolicitacaoGetAllQuery());

            if (result.IsError) { return Problem(result.Errors); }

            return Ok(result.Value);

        }

        [HttpPut("AlterarStatus")]
        public async Task<IActionResult> AlterarStatus([FromBody] SolicitacaoChangeStatusCommand command)
        {
            var result = await _mediator.Send(command);

            if (result.IsError) { return Problem(result.Errors); }

            return Ok(result.Value);

        }
    }
}
