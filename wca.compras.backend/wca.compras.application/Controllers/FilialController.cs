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
    public class FilialController : Controller
    {
        private IFilialService service;

        public FilialController(IFilialService service)
        {
            this.service = service;
        }

        [HttpGet]
        [Route("all")]
        public async Task<ActionResult<IList<FilialDto>>> GetAll()
        {
            try
            {
                var items = await service.GetAll();
                return Ok(items);
            }
            catch (ArgumentException ex)
            {
                return StatusCode((int)HttpStatusCode.InternalServerError, ex.Message);
            }
        }

        [HttpGet]
        [Route("ToList")]
        public async Task<ActionResult<IList<ListItem>>> List()
        {
            try
            {
                var items = await service.GetToList();
                return Ok(items);
            }
            catch (ArgumentException ex)
            {
                return StatusCode((int)HttpStatusCode.InternalServerError, ex.Message);
            }
            
        }


        [HttpPost]
        public async Task<ActionResult<PermissaoDto>> Create(CreateFilialDto createFilial)
        {
            try
            {
                var filial = await service.Create(createFilial);
                return Ok(filial);
            }
            catch (ArgumentException ex)
            {
                return StatusCode((int)HttpStatusCode.InternalServerError, ex.Message);
            }
            
        }

        [HttpPut]
        public async Task<ActionResult> Update([FromBody] FilialDto updateFilialDto)
        {
            
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest();
                }
                var result = await service.Update(updateFilialDto);
                if (result == null)
                {
                    return NotFound($"Filial íd: {updateFilialDto.Id}, não localizado!"); 
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
        public ActionResult<Pagination<FilialDto>> Paginate(int pageSize = 10, int page = 1, string? termo = "")
        {
            try
            {
                var items = service.Paginate(page, pageSize, termo);
                return Ok(items);
            }
            catch (Exception ex)
            {
                 return StatusCode((int)HttpStatusCode.InternalServerError, ex.Message);
            }
            
        }

    }
}
