using MediatR;
using Microsoft.AspNetCore.Mvc;
using wca.share.application.Features.Notificacoes.Queries;
using wca.share.application.Features.Notificacoes.Commands;


namespace wca.share.webapi.Controllers
{
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

        [HttpPut("MarcarComoLido")]
        public async Task<IActionResult> MarcarComoLido(NotificacaoMarcarLidoCommand command)
        {
            var result = await _mediator.Send(command);

            if (result.IsError) { return Problem(result.Errors); }

            return Ok(result.Value);
        }

        [HttpPost("EnviarNotificacao")]
        public async Task<IActionResult> EnviarNotificacao(NotificacaoSendCommand command)
        {
            var result = await _mediator.Send(command);

            if (result.IsError) { return Problem(result.Errors); }

            return Ok(result.Value);
        }

        [HttpGet("Paginar")]
        public async Task<IActionResult> ToPaginate([FromQuery] NotificacaoToPaginateQuery query)
        {
            var result = await _mediator.Send(query);

            if (result.IsError) { return Problem(result.Errors); }

            return Ok(result.Value);
        }

    }
}