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
    [Authorize("Bearer")]
    public class FilialController : Controller
    {
        private IFilialService service;

        public FilialController(IFilialService service)
        {
            this.service = service;
        }

        //[HttpGet]
        //[Route("all")]
        //public async Task<ActionResult<IList<FilialDto>>> GetAll()
        //{
        //    try
        //    {
        //        var items = await service.GetAll();
        //        return Ok(items);
        //    }
        //    catch (ArgumentException ex)
        //    {
        //        return StatusCode((int)HttpStatusCode.InternalServerError, ex.Message);
        //    }
        //}

        /// <summary>
        /// Retorna lista de filiais ativas para preenchimento de Listas e Combos
        /// </summary>
        /// <returns>items</returns>
        [HttpGet]
        [Route("ToList")]
        public async Task<ActionResult<IList<ListItem>>> List()
        {
            try
            {
                int filial = 1; //int.Parse(User.FindFirst("Filial").Value);
                var items = await service.GetToList(filial);
                return Ok(items);
            }
            catch (ArgumentException ex)
            {
                return StatusCode((int)HttpStatusCode.InternalServerError, ex.Message);
            }
            
        }

        /// <summary>
        /// Retorna lista de Clientes ativos relacionados ao usuário
        /// </summary>
        /// <returns>items</returns>
        [HttpGet]
        [Route("ListByAuthenticatedUser")]
        public async Task<ActionResult<IList<ListItem>>> ListByAuthenticatedUser()
        {
            try
            {
                int codigoUsuario = int.Parse(User.FindFirst("CodigoUsuario").Value);
                var items = await service.GetToListByUser (codigoUsuario);
                return Ok(items);
            }
            catch (ArgumentException ex)
            {
                return StatusCode((int)HttpStatusCode.InternalServerError, ex.Message);
            }
        }


        /// <summary>
        /// Cria uma nova filial
        /// </summary>
        /// <returns>Filial</returns>
        /// <param name="createFilial"></param>
        [HttpPost]
        public async Task<ActionResult<FilialDto>> Create(CreateFilialDto createFilial)
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

        /// <summary>
        /// Atualiza dados da filial
        /// </summary>
        /// <returns>Filial</returns>
        /// <param name="updateFilialDto"></param>
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

        /// <summary>
        /// Retorna lista de filial por paginação
        /// </summary>
        /// <returns>FilialDto</returns>
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
