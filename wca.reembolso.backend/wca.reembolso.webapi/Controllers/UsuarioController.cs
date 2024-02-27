using MediatR;
using Microsoft.AspNetCore.Mvc;
using wca.reembolso.application.Features.Clientes.Commands;
using wca.reembolso.application.Features.Usuarios.Commands;

namespace wca.reembolso.webapi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
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


    }
}
