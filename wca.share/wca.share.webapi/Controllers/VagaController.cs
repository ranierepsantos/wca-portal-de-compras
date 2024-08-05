using MediatR;
using Microsoft.AspNetCore.Mvc;
using wca.share.application.Features.Solicitacoes.Queries;
using wca.share.application.Features.Vagas.Commands;
using wca.share.application.Features.Vagas.Queries;

namespace wca.share.webapi.Controllers
{
    public class VagaController : ApiController
    {
        private readonly IMediator _mediator;

        public VagaController(IMediator mediator)
        {
            _mediator = mediator;
        }


        [HttpPost]
        public async Task<IActionResult> Create(VagaCreateCommand command)
        {
            var result = await _mediator.Send(command);

            return result.IsError ? Problem(result.Errors) : Ok(result.Value);
        }

        [HttpGet]
        public async Task<IActionResult> GetById([FromQuery] VagaFindByIdQuery command)
        {
            var result = await _mediator.Send(command);

            if (result.IsError) { return Problem(result.Errors); }

            return Ok(result.Value);
        }

        [HttpPut]
        public async Task<IActionResult> Update(VagaUpdateCommand updateCommand)
        {
            var result = await _mediator.Send(updateCommand);

            if (result.IsError) { return Problem(result.Errors); }

            return Ok(result.Value);
        }

        [HttpPost("Paginar")]
        public async Task<IActionResult> ToPaginate([FromBody] VagaPaginateQuery querie)
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

        //[HttpPut("AlterarStatus")]
        //public async Task<IActionResult> AlterarStatus([FromBody] VagaChangeStatusCommand command)
        //{
        //    var result = await _mediator.Send(command);

        //    if (result.IsError) { return Problem(result.Errors); }

        //    return Ok(result.Value);

        //}

    }
}
