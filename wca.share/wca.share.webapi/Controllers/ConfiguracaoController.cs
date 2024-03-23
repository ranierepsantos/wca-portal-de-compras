using MediatR;
using Microsoft.AspNetCore.Mvc;
using wca.share.application.Features.Clientes.Queries;
using wca.share.application.Features.Configuracoes.Commands;
using wca.share.application.Features.Configuracoes.Queries;

namespace wca.share.webapi.Controllers
{
    public class ConfiguracaoController : ApiController
    {
        private readonly IMediator _mediator;

        public ConfiguracaoController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet("GetAll")]
        public async Task<IActionResult> GetAll()
        {
            var result = await _mediator.Send(new ConfiguracaoListQuery());

            if (result.IsError) { return Problem(result.Errors); }

            return Ok(result.Value);
        }


        [HttpPut]
        public async Task<IActionResult> Update(ConfiguracaoUpdateCommand command)
        {
            var result = await _mediator.Send(command);

            if (result.IsError) { return Problem(result.Errors); }

            return Ok(result.Value);
        }


        [HttpGet("ByChave")]
        public async Task<IActionResult> GetByChave([FromQuery] ConfiguracaoGetByChaveQuery query)
        {
            var result = await _mediator.Send(query);

            if (result.IsError) { return Problem(result.Errors); }

            return Ok(result.Value);
        }

    }
}
