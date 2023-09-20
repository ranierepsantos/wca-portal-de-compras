using MediatR;
using Microsoft.AspNetCore.Mvc;
using wca.reembolso.application.Features.Notificacoes.Queries;
using wca.reembolso.application.Features.Solicitacoes.Queries;

namespace wca.reembolso.webapi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class NotificacaoController : ApiController
    {
        private readonly IMediator _mediator;

        public NotificacaoController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet("ListarPorUsuario")]
        public async Task<IActionResult> ListarPorColaboradorGestor([FromQuery] NotificacaoListByUserQuery query)
        {
            var result = await _mediator.Send(query);

            if (result.IsError) { return Problem(result.Errors); }

            return Ok(result.Value);
        }
    }
}
