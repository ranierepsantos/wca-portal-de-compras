using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Data;
using System.Net;
using wca.compras.domain.Dtos;
using wca.compras.domain.Email;
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

                var result = await service.aprovarRequisicao(aprovarRequisicaoDto, Request.Headers["origin"]);

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

                var result = await service.Update(updateRequisicaoDto, Request.Headers["origin"]);
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

                var result = await service.GetById(id);
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
        public ActionResult<Pagination<RequisicaoDto>> Paginate(int pageSize = 10, int page = 1, [FromQuery] int[] filial = null, int authUserId = 0, int clienteId = 0, int fornecedorId = 0, int usuarioId = 0, DateTime? dataInicio = null, DateTime? dataFim = null, [FromQuery] int[]? status = null)
        {
            try
            {
                if (dataInicio > dataFim || (dataInicio != null && dataFim is null) || (dataFim != null && dataInicio is null))
                {
                    return BadRequest(error: new { message = "Data início ou fim inválida!" });
                }
                
                var items = service.Paginate(filial, authUserId, page, pageSize, clienteId, usuarioId, fornecedorId, dataInicio, dataFim, status);
                return Ok(items);
            }
            catch (Exception ex)
            {
                return StatusCode((int)HttpStatusCode.InternalServerError, ex.Message);
            }

        }

        /// <summary>
        /// Retorna lista de Requisição por paginação
        /// </summary>
        /// <returns>RequisicaoDto</returns>
        [HttpGet]
        [Route("PaginateByUserContext/{pageSize}/{page}")]
        [Authorize("Bearer")]
        public ActionResult<Pagination<RequisicaoDto>> PaginateByUserContext(int pageSize = 10, int page = 1,[FromQuery] int[] filial = null, int clienteId = 0, int fornecedorId = 0, int usuarioId = 0, DateTime? dataInicio = null, DateTime? dataFim = null, [FromQuery] int[]? status = null)
        {
            try
            {
                if (dataInicio > dataFim || (dataInicio != null && dataFim is null) || (dataFim != null && dataInicio is null))
                {
                    return BadRequest(error: new { message = "Data início ou fim inválida!" });
                }
                int logedUserId = int.Parse(User.FindFirst("CodigoUsuario").Value);
                var items = service.Paginate(filial, logedUserId, page, pageSize, clienteId, usuarioId, fornecedorId,  dataInicio, dataFim, status);
                return Ok(items);
            }
            catch (Exception ex)
            {
                return StatusCode((int)HttpStatusCode.InternalServerError, ex.Message);
            }

        }


        [HttpGet]
        [Route("GerarRelatorio")]
        [Authorize("Bearer")]
        public async Task<ActionResult> ExportExcel([FromQuery] int [] filial = null, int clienteId = 0, int fornecedorId = 0, int usuarioId = 0,
                                                    DateTime? dataInicio = null, DateTime? dataFim = null, int authUserId = 0, [FromQuery] params int[] status)
        {
            try
            {
                if (dataInicio > dataFim || (dataInicio != null && dataFim is null) || (dataFim != null && dataInicio is null))
                {
                    return BadRequest(error: new { message = "Data início ou fim inválida!"});
                }

                Stream st = await service.ExportToExcel(filial, clienteId, fornecedorId, usuarioId, dataInicio, dataFim, authUserId, status);
                if (st == null)
                    return NoContent();

                return new FileStreamResult(st, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet") { FileDownloadName = $"Requisicoes_relatorio.xlsx" };

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
                string nomeUsuario = User.FindFirst("UsuarioNome").Value;
                var result = await service.Remove(id, nomeUsuario);

                if (!result) return NotFound($"Requisição, não localizado!");

                return Ok(result);
            }
            catch (Exception ex)
            {
                return StatusCode((int)HttpStatusCode.InternalServerError, ex.Message);
            }
        }

        /// <summary>
        /// Cria uma requisição com base numa requisição existente
        /// </summary>
        ///<param name="id"></param>
        [HttpPut]
        [Route("Duplicate/{id}")]
        [Authorize("Bearer")]
        public async Task<ActionResult> Duplicate(int id)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest();
                }
                string nomeUsuario = User.FindFirst("UsuarioNome").Value;
                int usuarioId = int.Parse(User.FindFirst("CodigoUsuario").Value);

                var result = await service.Duplicate(id, usuarioId,nomeUsuario, Request.Headers["origin"]);

                return Ok(result);
            }
            catch (Exception ex)
            {
                return StatusCode((int)HttpStatusCode.InternalServerError, ex.Message);
            }
        }

        /// <summary>
        /// Finalizar o Requisição, alterando seu status para Finalizado
        /// </summary>
        ///<param name="finalizarRequisicao"></param>
        [HttpPost]
        [Route("FinalizarPedido")]
        [Authorize("Bearer")]
        public async Task<ActionResult> Finalizar(FinalizarRequisicaoDto finalizarRequisicao)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest();
                }
                string nomeUsuario = User.FindFirst("UsuarioNome").Value;
                
                var result = await service.FinalizarPedido(finalizarRequisicao, nomeUsuario);

                if (result == false)
                {
                    return NotFound();
                }

                return Ok();
            }
            catch (Exception ex)
            {
                return StatusCode((int)HttpStatusCode.InternalServerError, ex.Message);
            }
        }

        /// <summary>
        /// Envia requisição para aprovação do fornecedor
        /// </summary>
        /// <returns>NoContent</returns>
        /// <param name="requisicaoId"></param>
        /// <param name="destinoEmail"></param>
        [HttpPut]
        [Route("SolicitarAprovacao/{requisicaoId}/{destinoEmail}")]
        [Authorize("Bearer")]
        public async Task<ActionResult> SolicitarAprovacaoFornecedor(int requisicaoId, EnumRequisicaoDestinoEmail destinoEmail)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest();
                }

                var result = await service.EnviarRequisicao(requisicaoId, destinoEmail, Request.Headers["origin"]);

                if (result == false)
                    return NotFound("Não há e-mail cadastrado para envio!");

                return NoContent();
            }
            catch (ArgumentException ex)
            {
                return StatusCode((int)HttpStatusCode.InternalServerError, ex.Message);
            }
        }
    }
}
