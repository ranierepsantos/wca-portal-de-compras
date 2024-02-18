using ErrorOr;
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
    }
}
