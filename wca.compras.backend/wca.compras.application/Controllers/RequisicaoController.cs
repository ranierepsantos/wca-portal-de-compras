using ClosedXML.Excel;
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
    public class RequisicaoController : Controller
    {
        private IRequisicaoService service;

        public RequisicaoController(IRequisicaoService service)
        {
            this.service = service;
        }

        [HttpGet]
        [Route("ExportExcel/{token}")]
        public async Task<ActionResult> ExportExcel(string token)
        {
            try
            {
                var requisicao = await service.GetByAprovacaoToken(token);

                Stream st = await service.ExportToExcel(requisicao.Id);

                return new FileStreamResult(st, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet") { FileDownloadName = $"WCAPedido_{requisicao.Id}.xlsx" };

            }
            catch (Exception ex)
            {
                return StatusCode((int)HttpStatusCode.InternalServerError, ex.Message);
            }
            
        }

        /// <summary>
        /// Cadastra uma nova Requisição
        /// </summary>
        /// <returns>NoContent</returns>
        /// <param name="createRequisicaoDto"></param>
        [HttpPost]
        [Authorize("Bearer")]
        public async Task<ActionResult> Create([FromBody] CreateRequisicaoDto createRequisicaoDto)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest();
                }

                var result = await service.Create(createRequisicaoDto, Request.Headers["origin"]);

                return Created("", result);
            }
            catch (ArgumentException ex)
            {
                return StatusCode((int)HttpStatusCode.InternalServerError, ex.Message);
            }
        }

        /// <summary>
        /// Aprova/Rejeita uma Requisição
        /// </summary>
        /// <returns>NoContent</returns>
        /// <param name="aprovarRequisicaoDto"></param>
        [HttpPost]
        [Route("AprovarReprovar")]
        public async Task<ActionResult> AprovarReprovar([FromBody] AprovarRequisicaoDto aprovarRequisicaoDto)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest();
                }

                var result = await service.aprovarRequisicao(aprovarRequisicaoDto);

                if (result == false)
                    return NotFound();


                return NoContent();
            }
            catch (ArgumentException ex)
            {
                return StatusCode((int)HttpStatusCode.InternalServerError, ex.Message);
            }
        }


        /// <summary>
        /// Atualiza dados da Requisição
        /// </summary>
        /// <returns>NoContent</returns>
        /// <param name="updateRequisicaoDto"></param>
        [HttpPut]
        [Authorize("Bearer")]
        public async Task<ActionResult> Update([FromBody] UpdateRequisicaoDto updateRequisicaoDto)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest();
                }

                int filial = int.Parse(User.FindFirst("Filial").Value);

                var result = await service.Update(filial, updateRequisicaoDto);
                if (result == null)
                {
                    return NotFound($"Requisição íd: {updateRequisicaoDto.Id}, não localizado!");
                }
                return Ok(result);
            }
            catch (ArgumentException ex)
            {
                return StatusCode((int)HttpStatusCode.InternalServerError, ex.Message);
            }
        }

        /// <summary>
        /// Busca Requisição pelo Id
        /// </summary>
        /// <returns>RequisicaoDto</returns>
        /// <param name="id"></param>
        [HttpGet]
        [Route("{id}")]
        [Authorize("Bearer")]
        public async Task<ActionResult<RequisicaoDto>> Get(int id)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest();
                }

                int filial = int.Parse(User.FindFirst("Filial").Value);

                var result = await service.GetById(filial, id);
                if (result == null)
                {
                    return NotFound($"Requisição íd: {id}, não localizado!");
                }
                return Ok(result);
            }
            catch (ArgumentException ex)
            {
                return StatusCode((int)HttpStatusCode.InternalServerError, ex.Message);
            }
        }

        /// <summary>
        /// Busca Requisição pelo token para aprovação/rejeição
        /// </summary>
        /// <returns>RequisicaoDto</returns>
        /// <param name="token"></param>
        [HttpGet]
        [Route("GetByToken/{token}")]
        public async Task<ActionResult<RequisicaoDto>> GetByToken(string token)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest();
                }

                var result = await service.GetByAprovacaoToken(token);
                if (result == null)
                {
                    return NotFound($"Requisição, não localizado!");
                }
                return Ok(result);
            }
            catch (ArgumentException ex)
            {
                return StatusCode((int)HttpStatusCode.InternalServerError, ex.Message);
            }
        }


        /// <summary>
        /// Retorna lista de Requisição por paginação
        /// </summary>
        /// <returns>RequisicaoDto</returns>
        [HttpGet]
        [Route("Paginate/{pageSize}/{page}")]
        [Authorize("Bearer")]
        public ActionResult<Pagination<RequisicaoDto>> Paginate(int pageSize = 10, int page = 1, int clienteId = 0, int fornecedorId = 0, int usuarioId = 0, EnumStatusRequisicao status = EnumStatusRequisicao.TODOS)
        {
            try
            {
                int filial = int.Parse(User.FindFirst("Filial").Value);
                var items = service.Paginate(filial, page, pageSize, clienteId, fornecedorId, usuarioId, status);
                return Ok(items);
            }
            catch (Exception ex)
            {
                return StatusCode((int)HttpStatusCode.InternalServerError, ex.Message);
            }

        }

        /// <summary>
        /// Altera o status da requisição para Cancelado
        /// </summary>
        /// <param name="id"></param>
        [HttpDelete]
        [Authorize("Bearer")]
        public async Task<ActionResult> Remove(int id)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest();
                }
                int filial = int.Parse(User.FindFirst("Filial").Value);
                string nomeUsuario = User.FindFirst("UsuarioNome").Value;
                var result = await service.Remove(filial, id, nomeUsuario);

                if (!result) return NotFound($"Requisição, não localizado!");

                return Ok(result);
            }
            catch (Exception ex)
            {
                return StatusCode((int)HttpStatusCode.InternalServerError, ex.Message);
            }
        }
    }
}
