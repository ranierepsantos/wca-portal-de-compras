using MediatR;
using Microsoft.AspNetCore.Mvc;
using wca.reembolso.application.Features.Usuarios.Commands;
using wca.reembolso.application.Features.Usuarios.Queries;

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

        [HttpGet("ListFilialByUsuario/{usuarioId}")]
        public async Task<IActionResult> ListFilialByUsuario([FromRoute] UsuarioGetFiliais command)
        {
            var result = await _mediator.Send(command);

            if (result.IsError) { return Problem(result.Errors); }

            return Ok(result.Value);
        }


    }
}
