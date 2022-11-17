using Microsoft.AspNetCore.Mvc;
using System.Net;
using wca.compras.domain.Interfaces.Services;
using wca.compras.domain.Util;
using wca.compras.domain.Dtos;

namespace wca.compras.webapi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PerfilController : Controller
    {
        private readonly IPerfilService _service;
        public PerfilController(IPerfilService profileService)
        {
            _service = profileService;
        }

        /// <summary>
        /// Cadastra um novo perfil e suas permissões
        /// </summary>
        /// <returns>Perfil com permissões</returns>
        /// <param name="perfilDto"></param>
        [HttpPost]
        public async Task<ActionResult> Add([FromBody] CreatePerfilDto perfilDto)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest();
                }

                var result = await _service.Create(perfilDto);
                return Created("", result);
            }
            catch (ArgumentException ex)
            {
                return StatusCode((int)HttpStatusCode.InternalServerError, ex.Message);
            }
        }


        /// <summary>
        /// Atualiza dados do perfil, atribui/remove permissão
        /// </summary>
        /// <returns>Perfil com permissões</returns>
        /// <param name="perfilDto"></param>
        [HttpPut]
        public async Task<ActionResult> Update([FromBody] UpdatePerfilDto perfilDto)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest();
                }

                var result = await _service.Update(perfilDto);
                if (result == null)
                {
                    return NotFound($"Perfil íd: {perfilDto.Id}, não localizado!");
                }
                return Ok(result);
            }
            catch (ArgumentException ex)
            {
                return StatusCode((int)HttpStatusCode.InternalServerError, ex.Message);
            }
        }

        /// <summary>
        /// Busca o perfil com suas respectivas permissões
        /// </summary>
        /// <returns>Perfil com permissões</returns>
        /// <param name="id"></param>
        [HttpGet]
        [Route("GetWithPermissions/{id}")]
        public async Task<ActionResult<PerfilPermissoesDto>> GetWithPermissions(string id)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest();
                }
                var result = await _service.GetWithPermissoes(id);
                if (result == null)
                {
                    return NotFound($"Perfil íd: {id}, não localizado!");
                }
                return Ok(result);
            }
            catch (ArgumentException ex)
            {
                return StatusCode((int)HttpStatusCode.InternalServerError, ex.Message);
            }
        }

        /// <summary>
        /// Retorna lista de perfil ativos para preenchimento de Listas e Combos
        /// </summary>
        /// <returns>Perfil com permissões</returns>
        [HttpGet]
        [Route("ToList")]
        public async Task<ActionResult<IList<ListItem>>> List()
        {
            var items = await _service.GetToList();
            return Ok(items);
        }

        /// <summary>
        /// Retorna Perfil por paginação
        /// </summary>
        /// <returns>Perfil</returns>
        [HttpGet]
        [Route("Paginate/{pageSize}/{page}")]
        public async Task<ActionResult<Pagination<PerfilDto>>> Paginate(int pageSize = 10, int page = 1, string? termo = "")
        {
            var items = await _service.Paginate(page, pageSize, termo);
            return Ok(items);
        }
    }
}
