using MediatR;
using Microsoft.AspNetCore.Mvc;
using wca.share.application.Features.MotivosContratacao.Commands;
using wca.share.application.Features.MotivosContratacao.Queries;

namespace wca.share.webapi.Controllers
{
    public class MotivoContratacaoController : ApiController
    {
        private readonly IMediator _mediator;

        public MotivoContratacaoController(IMediator mediator)
        {
            _mediator = mediator;
        }


        [HttpGet("Paginar")]
        public async Task<IActionResult> Paginar([FromQuery] MotivoContratacaoToPaginateQuery query)
        {
            var result = await _mediator.Send(query);

            if (result.IsError) { return Problem(result.Errors); }

            return Ok(result.Value);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] MotivoContratacaoCreateCommand command)
        {
            var result = await _mediator.Send(command);

            if (result.IsError) { return Problem(result.Errors); }

            return Ok(result.Value);
        }

        [HttpPut]
        public async Task<IActionResult> Update([FromBody] MotivoContratacaoUpdateCommand command)
        {
            var result = await _mediator.Send(command);

            if (result.IsError) { return Problem(result.Errors); }

            return Ok(result.Value);
        }

        [HttpGet]
        public async Task<IActionResult> GetById([FromQuery] MotivoContratacaoByIdQuery query)
        {
            var result = await _mediator.Send(query);

            if (result.IsError) { return Problem(result.Errors); }

            return Ok(result.Value);
        }

        [HttpGet("ToComboList")]
        public async Task<IActionResult> ListarMotivoContratacaos()
        {
            var result = await _mediator.Send(new MotivoContratacaoListQuery());
            
            if (result.IsError) { return Problem(result.Errors); }

            return Ok(result.Value);

        }
    }
}
