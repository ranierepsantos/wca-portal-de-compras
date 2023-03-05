using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using wca.compras.domain.Dtos;
using wca.compras.domain.Entities;
using wca.compras.domain.Interfaces.Services;

namespace wca.compras.webapi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize("Bearer")]
    public class ConfiguracaoController : Controller
    {
        private readonly IConfiguracaoService service;

        public ConfiguracaoController(IConfiguracaoService service)
        {
            this.service = service;
        }


        /// <summary>
        /// Atualiza dados do tipo de fornecimento
        /// </summary>
        /// <returns>configuracao</returns>
        /// <param name="configuracao"></param>
        [HttpPut]
        public async Task<ActionResult> Update([FromBody] UpdateConfiguracaoDto configuracao)
        {

            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest();
                }
                var result = await service.Update(configuracao);
                return NoContent();
            }
            catch (ArgumentException ex)
            {
                return StatusCode((int)HttpStatusCode.InternalServerError, ex.Message);
            }
        }

        /// <summary>
        /// Retorna todas as configurações
        /// </summary>
        /// <returns>Lista de configurações</returns>
        [HttpGet]
        
        public async Task<ActionResult<IList<Configuracao>>> GetAll()
        {
            var items = await service.GetAll();
            return Ok(items);
        }

    }
}
