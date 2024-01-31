using MediatR;
using Microsoft.AspNetCore.Mvc;
using wca.share.application.Features.Usuarios.Commands;

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

    }
}
