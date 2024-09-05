using MediatR;
using Microsoft.AspNetCore.Mvc;
using wca.share.application.Features.Funcoes.Commands;
using wca.share.application.Features.Funcoes.Queries;

namespace wca.share.webapi.Controllers
{
    public class FuncaoController : ApiController
    {
        private readonly IMediator _mediator;

        public FuncaoController(IMediator mediator)
        {
            _mediator = mediator;
        }


        [HttpGet("Paginar")]
        public async Task<IActionResult> Paginar([FromQuery] FuncaoToPaginateQuery query)
        {
            var result = await _mediator.Send(query);

            if (result.IsError) { return Problem(result.Errors); }

            return Ok(result.Value);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] FuncaoCreateCommand command)
        {
            var result = await _mediator.Send(command);

            if (result.IsError) { return Problem(result.Errors); }

            return Ok(result.Value);
        }

        [HttpPut]
        public async Task<IActionResult> Update([FromBody] FuncaoUpdateCommand command)
        {
            var result = await _mediator.Send(command);

            if (result.IsError) { return Problem(result.Errors); }

            return Ok(result.Value);
        }

        [HttpGet]
        public async Task<IActionResult> GetById([FromQuery] FuncaoByIdQuery query)
        {
            var result = await _mediator.Send(query);

            if (result.IsError) { return Problem(result.Errors); }

            return Ok(result.Value);
        }

        [HttpGet("ToComboList")]
        public async Task<IActionResult> ListarFuncaos()
        {
            var result = await _mediator.Send(new FuncaoListQuery());

            if (result.IsError) { return Problem(result.Errors); }

            return Ok(result.Value);

        }
    }
}
