using MediatR;
using Microsoft.AspNetCore.Mvc;
using wca.share.application.Features.Clientes.Commands;
using wca.share.application.Features.Clientes.Queries;

namespace wca.share.webapi.Controllers
{
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

        [HttpGet("ListarClientesPorUsuario")]
        public async Task<IActionResult> ListClientesByUsuario([FromQuery] ClienteByUserIdQuerie querie)
        {
            var result = await _mediator.Send(querie);

            if (result.IsError) { return Problem(result.Errors); }

            return Ok(result.Value);
        }

        [HttpPost("ListarCentroCustoPorCliente")]
        public async Task<IActionResult> ListarCentroCustoPorCliente([FromBody] CentrodeCustoByClienteQuery query)
        {
            var result = await _mediator.Send(query);

            if (result.IsError) { return Problem(result.Errors); }

            return Ok(result.Value);
        }

        [HttpGet("CreateFromGI")]
        public async Task<IActionResult> CreateFromGI()
        {
            var result = await _mediator.Send(new ClienteCreateFromIntegration());

            if (result.IsError) { return Problem(result.Errors); }

            return Ok(result.Value);
        }
    }
}
