using MediatR;
using Microsoft.AspNetCore.Mvc;
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


    }
}
