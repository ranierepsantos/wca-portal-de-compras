using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using wca.compras.domain.Dtos;
using wca.compras.domain.Entities;
using wca.compras.domain.Interfaces.Services;
using wca.compras.domain.Util;

namespace wca.compras.webapi.Controllers
{

    [ApiController]
    [Route("api/[controller]")]
    public class RecorrenciaController : Controller
    {
        private readonly IRecorrenciaService service;

        public RecorrenciaController(IRecorrenciaService service)
        {
            this.service = service;
        }

        /// <summary>
        /// Cadastra uma nova Recorrencia
        /// </summary>
        /// <returns>NoContent</returns>
        /// <param name="createRecorrenciaDto"></param>
        [HttpPost]
        [Authorize("Bearer")]
        public async Task<ActionResult> Create([FromBody] CreateRecorrenciaDto createRecorrenciaDto)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest();
                }

                var result = await service.Create(createRecorrenciaDto, Request.Headers["origin"]);

                return Created("", result);
            }
            catch (ArgumentException ex)
            {
                return StatusCode((int)HttpStatusCode.InternalServerError, ex.Message);
            }
        }

        /// <summary>
        /// Retorna lista de Recorrência por paginação
        /// </summary>
        /// <returns>Pagination<RecorrrenciaDto/></returns>
        [HttpGet]
        [Route("Paginate/{pageSize}/{page}")]
        [Authorize("Bearer")]
        public ActionResult<Pagination<RecorrenciaDto>> Paginate(int pageSize = 10, int page = 1, int clienteId = 0, int fornecedorId = 0, int usuarioId = 0)
        {
            try
            {
                int filial = int.Parse(User.FindFirst("Filial").Value);
                var items = service.Paginate(filial, page, pageSize, clienteId, fornecedorId, usuarioId);
                return Ok(items);
            }
            catch (Exception ex)
            {
                return StatusCode((int)HttpStatusCode.InternalServerError, ex.Message);
            }

        }

        /// <summary>
        /// Retorna os log's de execução da recorrência
        /// </summary>
        /// <param name="recorrenciaId">Id da recorrência</param>
        /// <param name="pageSize">Quantidade de items por pagína</param>
        /// <param name="page">Número da pagina</param>

        [HttpGet]
        [Route("Log/{recorrenciaId}/{pageSize}/{page}")]
        [Authorize("Bearer")]
        public ActionResult<Pagination<RecorrenciaLogDto>> PaginateLog(int recorrenciaId, int pageSize = 10, int page = 1)
        {
            try
            {
                var items = service.PaginationLog(recorrenciaId, page, pageSize);
                return Ok(items);
            }
            catch (Exception ex)
            {
                return StatusCode((int)HttpStatusCode.InternalServerError, ex.Message);
            }

        }

        /// <summary>
        /// Executa Requisições agendadas para o dia
        /// </summary>
        [HttpGet]
        [Route("ExecuteSchedule")]
        //[Authorize("Bearer")]
        public async Task<ActionResult> ExecuteScheduleAsync()
        {
            try
            {
                await service.ExecuteScheduleAsync();
                return Ok();
            }
            catch (Exception ex)
            {
                return StatusCode((int)HttpStatusCode.InternalServerError, ex.Message);
            }
        }

        /// <summary>
        /// Busca Recorrência pelo Id
        /// </summary>
        /// <returns>RequisicaoDto</returns>
        /// <param name="id"></param>
        [HttpGet]
        [Route("{id}")]
        [Authorize("Bearer")]
        public async Task<ActionResult<RecorrenciaDto>> Get(int id)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest();
                }

                int filial = 1; //int.Parse(User.FindFirst("Filial").Value);

                var result = await service.GetById(filial, id);
                if (result == null)
                {
                    return NotFound($"Recorrência código: {id}, não localizado!");
                }
                return Ok(result);
            }
            catch (ArgumentException ex)
            {
                return StatusCode((int)HttpStatusCode.InternalServerError, ex.Message);
            }
        }

        /// <summary>
        /// Atualiza dados da Recorrência
        /// </summary>
        /// <returns>NoContent</returns>
        /// <param name="updateRecorrenciaDto"></param>
        [HttpPut]
        [Authorize("Bearer")]
        public async Task<ActionResult> Update([FromBody] UpdateRecorrenciaDto updateRecorrenciaDto)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest();
                }

                int filial = int.Parse(User.FindFirst("Filial").Value);

                var result = await service.Update(filial, updateRecorrenciaDto);
                if (result == null)
                {
                    return NotFound($"Requisição íd: {updateRecorrenciaDto.Id}, não localizado!");
                }
                return Ok(result);
            }
            catch (ArgumentException ex)
            {
                return StatusCode((int)HttpStatusCode.InternalServerError, ex.Message);
            }
        }

        /// <summary>
        /// Inativa/Ativa uma recorrência
        /// </summary>
        /// <param name="enableRecorrencia"></param>
        [HttpPut]
        [Authorize("Bearer")]
        [Route("EnabledDisabled")]
        public async Task<ActionResult> EnabledDisabled(EnabledRecorrenciaDto enableRecorrencia)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest();
                }
                int filial = int.Parse(User.FindFirst("Filial").Value);
                
                var result = await service.EnabledDisabled(filial, enableRecorrencia);

                if (!result) return NotFound($"Recorrência código {enableRecorrencia.Id}, não localizada!");

                return Ok(result);
            }
            catch (Exception ex)
            {
                return StatusCode((int)HttpStatusCode.InternalServerError, ex.Message);
            }
        }

    }
}
