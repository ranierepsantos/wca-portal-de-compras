using MediatR;
using Microsoft.AspNetCore.Mvc;
using wca.share.application.Features.Filiais.Command;

namespace wca.share.webapi.Controllers
{
    public class FilialController : ApiController
    {
        private readonly IMediator _mediator;

        public FilialController(IMediator mediator)
        {
            _mediator = mediator;
        }
        [HttpPost("CreateUpdate")]
        public async Task<IActionResult> UpdateCreate(FilialCreateUpdateCommand command)
        {
            var result = await _mediator.Send(command);

            if (result.IsError) { return Problem(result.Errors); }

            return Ok(result.Value);
        }

    }
}
