using Microsoft.AspNetCore.Mvc;
using System.Net;
using wca.compras.domain.Interfaces.Services;
using wca.compras.domain.Util;
using static wca.compras.domain.Dtos.PermissaoDtos;

namespace wca.compras.webapi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PermissaoController : Controller
    {
        private IPermissaoService service;

        public PermissaoController(IPermissaoService service)
        {
            this.service = service;
        }

        [HttpGet]
        [Route("all")]
        public async Task<ActionResult<IList<PermissaoDto>>> GetAll()
        {
            var items = await service.GetAll();
            return Ok(items);
        }

        [HttpGet]
        [Route("ToList")]
        public async Task<ActionResult<IList<ListItem>>> List()
        {
            var items = await service.GetToList();
            return Ok(items);
        }

        [HttpPost]
        public async Task<ActionResult<PermissaoDto>> Create(CreatePermissaoDto createPermissao)
        {
            var permission = await service.Create(createPermissao);
            return Ok(permission);
        }

        [HttpPut]
        public async Task<ActionResult> Update([FromBody] UpdatePermissaoDto updatePermissaoDto)
        {
            try
            {
                await service.Update(updatePermissaoDto);
                return NoContent();
            }
            catch (ArgumentException ex)
            {
                return StatusCode((int)HttpStatusCode.InternalServerError, ex.Message);
            }
        }

    }
}
