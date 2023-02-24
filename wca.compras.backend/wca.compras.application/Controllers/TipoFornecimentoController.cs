using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using wca.compras.domain.Dtos;
using wca.compras.domain.Interfaces.Services;
using wca.compras.domain.Util;

namespace wca.compras.webapi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize("Bearer")]
    public class TipoFornecimentoController : Controller
    {
        private readonly ITipoFornecimentoService service;

        public TipoFornecimentoController(ITipoFornecimentoService service)
        {
            this.service = service;
        }

        /// <summary>
        /// Retorna lista de tipos de fornecimento ativos para preenchimento de Listas e Combos
        /// </summary>
        /// <returns>Perfil com permissões</returns>
        [HttpGet]
        [Route("ToList")]
        public async Task<ActionResult<IList<ListItem>>> List()
        {
            var items = await service.GetToList();
            return Ok(items);
        }

        /// <summary>
        /// Cadastra um novo tipo de fornecimento
        /// </summary>
        /// <returns>Tipo de Fornecimento</returns>
        /// <param name="createTipoFornecimentoDto"></param>
        [HttpPost]
        public async Task<ActionResult<TipoFornecimentoDto>> Create(CreateTipoFornecimentoDto createTipoFornecimentoDto)
        {
            var tipo = await service.Create(createTipoFornecimentoDto);
            return Ok(tipo);
        }


        /// <summary>
        /// Atualiza dados do tipo de fornecimento
        /// </summary>
        /// <returns>Tipo de Fornecimento</returns>
        /// <param name="updateTipoFornecimentoDto"></param>
        [HttpPut]
        public async Task<ActionResult> Update([FromBody] TipoFornecimentoDto updateTipoFornecimentoDto)
        {

            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest();
                }
                var result = await service.Update(updateTipoFornecimentoDto);
                if (result == null)
                {
                    return NotFound($"Tipo Fornecimento íd: {updateTipoFornecimentoDto.Id}, não localizado!");
                }
                return Ok(result);
            }
            catch (ArgumentException ex)
            {
                return StatusCode((int)HttpStatusCode.InternalServerError, ex.Message);
            }
        }


        /// <summary>
        /// Retorna lista de tipos de fornecimento (categorias) por paginação
        /// </summary>
        /// <returns>TipoFornecimentoDto</returns>
        [HttpGet]
        [Route("Paginate/{pageSize}/{page}")]
        public ActionResult<Pagination<TipoFornecimentoDto>> Paginate(int pageSize = 10, int page = 1, string? termo = "")
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
