using MediatR;
using Microsoft.AspNetCore.Mvc;
using wca.share.application.Features.Usuarios.Commands;
using wca.share.application.Features.Usuarios.Queries;

namespace wca.share.webapi.Controllers
{
    public class UsuarioController : ApiController
    {
        private readonly IMediator _mediator;

        public UsuarioController(IMediator mediator)
        {
            _mediator = mediator;
        }
        [HttpPost("CreateUpdate")]
        public async Task<IActionResult> UpdateCreate(UsuarioCreateUpdateCommand command)
        {
            var result = await _mediator.Send(command);

            if (result.IsError) { return Problem(result.Errors); }

            return Ok(result.Value);
        }

        [HttpPost("RelacionarUsuarioCentroCusto")]
        public async Task<IActionResult> RelacionarUsuarioCentroCusto(UsuarioCentroCustoAttachCommand usuarioCentroCustoAttachCommand)
        {
            var result = await _mediator.Send(usuarioCentroCustoAttachCommand);

            if (result.IsError) { return Problem(result.Errors); }

            return Ok(result.Value);
        }

        [HttpGet("ListarCentroCusto/{UsuarioId}/{ClienteId}")]
        public async Task<IActionResult> ListarCentroCusto([FromRoute] UsuarioListarCentrodeCustoQuery query)
        {
            var result = await _mediator.Send(query);

            if (result.IsError) { return Problem(result.Errors); }

            return Ok(result.Value);
        }


        [HttpGet("ListarPorCentroCusto/{CentroCustoId}")]
        public async Task<IActionResult> ListarPorCentroCusto([FromRoute] UsuarioListarPorCentrodeCustoQuery query)
        {
            var result = await _mediator.Send(query);

            if (result.IsError) { return Problem(result.Errors); }

            return Ok(result.Value);
        }
    }
}
