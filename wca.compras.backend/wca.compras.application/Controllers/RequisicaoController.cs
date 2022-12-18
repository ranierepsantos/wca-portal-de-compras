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
                int userId = int.Parse(User.FindFirst("CodigoUsuario").Value);

                var result = await service.Create(userId, createRequisicaoDto);

                return Created("", result);
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
                int userId = int.Parse(User.FindFirst("CodigoUsuario").Value);

                var result = await service.Update(filial, userId, updateRequisicaoDto);
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

                int filial =int.Parse(User.FindFirst("Filial").Value);

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
                var items = service.Paginate(filial, page, pageSize,clienteId, fornecedorId, usuarioId, status);
                return Ok(items);
            }
            catch (Exception ex)
            {
                return StatusCode((int)HttpStatusCode.InternalServerError, ex.Message);
            }

        }
    }
}
