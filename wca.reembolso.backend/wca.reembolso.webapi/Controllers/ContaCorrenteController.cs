using MediatR;
using Microsoft.AspNetCore.Mvc;
using wca.reembolso.application.Features.Clientes.Commands;
using wca.reembolso.application.Features.Clientes.Queries;
using wca.reembolso.application.Features.Conta.Commands;
using wca.reembolso.application.Features.Conta.Queries;

namespace wca.reembolso.webapi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    
    public class ContaCorrenteController : ApiController
    {
        private readonly IMediator _mediator;

        public ContaCorrenteController(IMediator mediator) => _mediator = mediator;

        [HttpGet]
        public async Task<IActionResult> GetByUser([FromQuery] ContaCorrenteByUsuarioQuery command) 
        {
            
            var result = await _mediator.Send(command);
        
            if (result.IsError) { return Problem(result.Errors); }

            return Ok(result.Value);
        }

        [HttpPost]
        public async Task<IActionResult> Create(ContaCorrenteCreateUpdateCommand command)
        {
            var result = await _mediator.Send(command);

            if (result.IsError) { return Problem(result.Errors); }

            return Ok(result.Value);
        }

    }
}
