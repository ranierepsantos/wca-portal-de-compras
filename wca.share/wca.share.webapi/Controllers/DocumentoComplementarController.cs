using MediatR;
using Microsoft.AspNetCore.Mvc;
using wca.share.application.Features.DocumentoComplementares.Commands;
using wca.share.application.Features.DocumentoComplementares.Queries;

namespace wca.share.webapi.Controllers
{
    public class DocumentoComplementarController : ApiController
    {
        private readonly IMediator _mediator;

        public DocumentoComplementarController(IMediator mediator)
        {
            _mediator = mediator;
        }


        [HttpGet("Paginar")]
        public async Task<IActionResult> Paginar([FromQuery] DocumentoComplementarToPaginateQuery query)
        {
            var result = await _mediator.Send(query);

            if (result.IsError) { return Problem(result.Errors); }

            return Ok(result.Value);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] DocumentoComplementarCreateCommand command)
        {
            var result = await _mediator.Send(command);

            if (result.IsError) { return Problem(result.Errors); }

            return Ok(result.Value);
        }

        [HttpPut]
        public async Task<IActionResult> Update([FromBody] DocumentoComplementarUpdateCommand command)
        {
            var result = await _mediator.Send(command);

            if (result.IsError) { return Problem(result.Errors); }

            return Ok(result.Value);
        }

        [HttpGet]
        public async Task<IActionResult> GetById([FromQuery] DocumentoComplementarByIdQuery query)
        {
            var result = await _mediator.Send(query);

            if (result.IsError) { return Problem(result.Errors); }

            return Ok(result.Value);
        }

        [HttpGet("ToComboList")]
        public async Task<IActionResult> ListarDocumentoComplementars()
        {
            var result = await _mediator.Send(new DocumentoComplementarListQuery());

            if (result.IsError) { return Problem(result.Errors); }

            return Ok(result.Value);

        }
    }
}
