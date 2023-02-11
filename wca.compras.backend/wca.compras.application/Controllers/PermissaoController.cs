using Microsoft.AspNetCore.Mvc;
using System.Net;
using wca.compras.domain.Interfaces.Services;
using wca.compras.domain.Util;
using wca.compras.domain.Dtos;
using Microsoft.AspNetCore.Authorization;

namespace wca.compras.webapi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    //[Authorize("Bearer")]
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
                if (!ModelState.IsValid)
                {
                    return BadRequest();
                }
                var result = await service.Update(updatePermissaoDto);
                if (result == null)
                {
                    return NotFound($"Permissão íd: {updatePermissaoDto.Id}, não localizado!"); 
                }
                return Ok(result);
            }
            catch (ArgumentException ex)
            {
                return StatusCode((int)HttpStatusCode.InternalServerError, ex.Message);
            }
        }

        [HttpGet]
        [Route("Paginate/{pageSize}/{page}")]
        public ActionResult<Pagination<PermissaoDto>> Paginate(int pageSize = 10, int page = 1, string? termo = "")
        {
            var items =  service.Paginate(page, pageSize,termo);
            return Ok(items);
        }

    }
}
