using MediatR;
using Microsoft.AspNetCore.Mvc;
using wca.share.application.Features.Solicitacoes.Commands;
using wca.share.application.Features.Solicitacoes.Queries;

namespace wca.share.webapi.Controllers
{
    public class SolicitacaoController : ApiController
    {
        private readonly IMediator _mediator;

        public SolicitacaoController(IMediator mediator)
        {
            _mediator = mediator;
        }


        [HttpPost]
        public async Task<IActionResult> Create(SolicitacaoCreateCommand command)
        {
            var result = await _mediator.Send(command);

            return result.IsError ? Problem(result.Errors) : Ok(result.Value);
        }

        [HttpGet]
        public async Task<IActionResult> GetById([FromQuery] SolicitacaoByIdQuerie command)
        {
            var result = await _mediator.Send(command);

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

        [HttpPost("Paginar")]
        public async Task<IActionResult> ToPaginate([FromBody] SolicitacaoPaginateQuery querie)
        {
            var result = await _mediator.Send(querie);

            if (result.IsError) { return Problem(result.Errors); }

            return Ok(result.Value);
        }

        [HttpGet("ListarStatusSolicitacao")]
        public async Task<IActionResult> ListarStatusSolicitacao()
        {
            var result = await _mediator.Send(new StatusSolicitacaoListQuery());

            if (result.IsError) { return Problem(result.Errors); }

            return Ok(result.Value);

        }

        [HttpGet("ListarMotivoDemissao")]
        public async Task<IActionResult> ListarMotivoDemissao()
        {
            var result = await _mediator.Send(new MotivoDemissaoListQuery());

            if (result.IsError) { return Problem(result.Errors); }

            return Ok(result.Value);

        }

        [HttpGet("ListarAssuntos")]
        public async Task<IActionResult> ListarAssuntos()
        {
            var result = await _mediator.Send(new AssuntoListQuery());

            if (result.IsError) { return Problem(result.Errors); }

            return Ok(result.Value);

        }

        [HttpGet("ListarTipoFerias")]
        public async Task<IActionResult> ListarTipoFerias()
        {
            var result = await _mediator.Send(new TipoFeriasListQuery());

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
