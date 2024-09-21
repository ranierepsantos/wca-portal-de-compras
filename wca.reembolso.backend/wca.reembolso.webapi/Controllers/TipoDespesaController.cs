using MediatR;
using Microsoft.AspNetCore.Mvc;
using wca.reembolso.application.Features.TiposDespesa.Commands;
using wca.reembolso.application.Features.TiposDespesa.Queries;

namespace wca.reembolso.webapi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TipoDespesaController : ApiController
    {
        private readonly IMediator _mediator;

        public TipoDespesaController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet]
        public async Task<IActionResult> GetById([FromQuery] TipoDespesaByIdQuerie command) 
        {
            var result = await _mediator.Send(command);
        
            if (result.IsError) { return Problem(result.Errors); }

            return Ok(result.Value);
        }

        [HttpPost]
        public async Task<IActionResult> Create(TipoDespesaCreateCommand command)
        {
            var result = await _mediator.Send(command);

            if (result.IsError) { return Problem(result.Errors); }

            return Ok(result.Value);
        }

        [HttpGet("Paginar")]
        public async Task<IActionResult> ToPaginate([FromQuery] TipoDespesaPaginateQuery querie)
        {
            var result = await _mediator.Send(querie);

            if (result.IsError) { return Problem(result.Errors); }

            return Ok(result.Value);
        }

        [HttpGet("ToComboList")]
        public async Task<IActionResult> ToComboList([FromQuery] TipoDespesaToComboListQuerie query)
        {
            var result = await _mediator.Send(query);

            if (result.IsError) { return Problem(result.Errors); }

            return Ok(result.Value);
        }

        [HttpPut]
        public async Task<IActionResult> Update(TipoDespesaUpdateCommand command)
        {
            var result = await _mediator.Send(command);

            if (result.IsError) { return Problem(result.Errors); }

            return Ok(result.Value);
        }

    }
}
