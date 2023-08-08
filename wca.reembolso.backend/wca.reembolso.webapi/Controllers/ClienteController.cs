using MediatR;
using Microsoft.AspNetCore.Mvc;
using wca.reembolso.application.Features.Clientes.Commands;
using wca.reembolso.application.Features.Clientes.Queries;

namespace wca.reembolso.webapi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    
    public class ClienteController : ApiController
    {
        private readonly IMediator _mediator;

        public ClienteController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet]
        public async Task<IActionResult> GetById([FromQuery] ClienteByIdQuerie command) 
        {
            //int filial = int.Parse(User.FindFirst("Filial").Value);

            var result = await _mediator.Send(command);
        
            if (result.IsError) { return Problem(result.Errors); }

            return Ok(result.Value);
        }

        [HttpPost]
        public async Task<IActionResult> Create(ClienteCreateCommand cliente)
        {
            var result = await _mediator.Send(cliente);

            if (result.IsError) { return Problem(result.Errors); }

            return Ok(result.Value);
        }

        [HttpGet("Paginar")]
        public async Task<IActionResult> ToPaginate([FromQuery] ClientePaginateQuery querie)
        {
            var result = await _mediator.Send(querie);

            if (result.IsError) { return Problem(result.Errors); }

            return Ok(result.Value);
        }

        [HttpGet("ToComboList")]
        public async Task<IActionResult> ToComboList([FromQuery] ClienteToComboListQuerie querie)
        {
            var result = await _mediator.Send(querie);

            if (result.IsError) { return Problem(result.Errors); }

            return Ok(result.Value);
        }


        [HttpPut]
        public async Task<IActionResult> Update(ClienteUpdateCommand cliente)
        {
            var result = await _mediator.Send(cliente);

            if (result.IsError) { return Problem(result.Errors); }

            return Ok(result.Value);
        }

        [HttpGet("ListarClientesPorUsuario")]
        public async Task<IActionResult> ListClientesByUsuario([FromQuery] ClienteByUserIdQuerie querie)
        {
            var result = await _mediator.Send(querie);

            if (result.IsError) { return Problem(result.Errors); }

            return Ok(result.Value);
        }

        [HttpPost("RelacionarClienteUsuario")]
        public async Task<IActionResult> RelacionarClienteUsuario(ClienteUsuarioAttachCommand clienteUsuario)
        {
            var result = await _mediator.Send(clienteUsuario);

            if (result.IsError) { return Problem(result.Errors); }

            return Ok(result.Value);
        }

        [HttpGet("ListarUsuariosPorCliente")]
        public async Task<IActionResult> ListClientesByUsuario([FromQuery] ClienteUsersByClienteQuerie querie)
        {
            var result = await _mediator.Send(querie);

            if (result.IsError) { return Problem(result.Errors); }

            return Ok(result.Value);
        }
    }
}
