using MediatR;
using Microsoft.AspNetCore.Mvc;
using wca.share.application.Features.Funcionarios.Commands;
using wca.share.application.Features.Funcionarios.Queries;

namespace wca.share.webapi.Controllers
{
    public class FuncionarioController : ApiController
    {
        private readonly IMediator _mediator;

        public FuncionarioController(IMediator mediator)
        {
            _mediator = mediator;
        }


        [HttpGet("ListByClienteToCombo")]
        public async Task<IActionResult> ListByClienteToCombo([FromQuery] FuncionarioListByClienteQuery query)
        {
            var result = await _mediator.Send(query);

            if (result.IsError) { return Problem(result.Errors); }

            return Ok(result.Value);
        }

        [HttpPost("Paginar")]
        public async Task<IActionResult> Paginar([FromBody] FuncionarioToPaginateQuery query)
        {
            var result = await _mediator.Send(query);

            if (result.IsError) { return Problem(result.Errors); }

            return Ok(result.Value);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] FuncionarioCreateCommand command)
        {
            var result = await _mediator.Send(command);

            if (result.IsError) { return Problem(result.Errors); }

            return Ok(result.Value);
        }

        [HttpPut]
        public async Task<IActionResult> Update([FromBody] FuncionarioUpdateCommand command)
        {
            var result = await _mediator.Send(command);

            if (result.IsError) { return Problem(result.Errors); }

            return Ok(result.Value);
        }

        [HttpGet]
        public async Task<IActionResult> GetById([FromQuery] FuncionarioByIdQuery query)
        {
            var result = await _mediator.Send(query);

            if (result.IsError) { return Problem(result.Errors); }

            return Ok(result.Value);
        }

        [HttpGet("CreateFromGI")]
        public async Task<IActionResult> CreateFromGI()
        {
            var result = await _mediator.Send(new FuncionarioCreateFromGICommand());
            
            if (result.IsError) { return Problem(result.Errors); }

            return Ok(result.Value);
        }
    }
}
