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
    public class ClienteController : Controller
    {
        private IClienteService service;

        public ClienteController(IClienteService service)
        {
            this.service = service;
        }

        /// <summary>
        /// Cadastra um novo cliente
        /// </summary>
        /// <returns>NoContent</returns>
        /// <param name="clienteDto"></param>
        [HttpPost]
        public async Task<ActionResult> Create([FromBody] CreateClienteDto clienteDto)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest();
                }
                var result = await service.Create(clienteDto);
                return Created("", result);
            }
            catch (ArgumentException ex)
            {
                return StatusCode((int)HttpStatusCode.InternalServerError, ex.Message);
            }
        }

        /// <summary>
        /// Atualiza dados do cliente
        /// </summary>
        /// <returns>Cliente</returns>
        /// <param name="clienteDto"></param>
        [HttpPut]
        public async Task<ActionResult> Update([FromBody] UpdateClienteDto clienteDto)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest();
                }
                int filial = int.Parse(User.FindFirst("Filial").Value);

                var result = await service.Update(filial,clienteDto);
                if (result == null)
                {
                    return NotFound($"Perfil íd: {clienteDto.Id}, não localizado!");
                }
                return Ok(result);
            }
            catch (ArgumentException ex)
            {
                return StatusCode((int)HttpStatusCode.InternalServerError, ex.Message);
            }
        }

        /// <summary>
        /// Busca cliente pelo Id
        /// </summary>
        /// <returns>Cliente</returns>
        /// <param name="id"></param>
        [HttpGet]
        [Route("{id}")]
        public async Task<ActionResult<ClienteDto>> Get(int id)
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
                    return NotFound($"Cliente íd: {id}, não localizado!");
                }
                return Ok(result);
            }
            catch (ArgumentException ex)
            {
                return StatusCode((int)HttpStatusCode.InternalServerError, ex.Message);
            }
        }

        /// <summary>
        /// Retorna lista de Clientes ativos para preenchimento de Listas e Combos
        /// </summary>
        /// <returns>items</returns>
        /// <param name="filial"></param>
        [HttpGet]
        [Route("ToList/{filial}")]
        public async Task<ActionResult<IList<ListItem>>> List(int filial)
        {
            try
            {
                var items = await service.GetToList(filial);
                return Ok(items);
            }
            catch (ArgumentException ex)
            {
                return StatusCode((int)HttpStatusCode.InternalServerError, ex.Message);
            }
        }

        /// <summary>
        /// Retorna lista de Clientes por paginação
        /// </summary>
        /// <returns>ClienteDto</returns>
        [HttpGet]
        [Route("Paginate/{pageSize}/{page}")]
        public ActionResult<Pagination<ClienteDto>> Paginate(int pageSize = 10, int page = 1, string? termo = "")
        {
            try
            {
                int filial = int.Parse(User.FindFirst("Filial").Value);
                var items = service.Paginate(filial, page, pageSize, termo);
                return Ok(items);
            }
            catch (Exception ex)
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
        public async Task<ActionResult<IList<ClienteDto>>> ListByAuthenticatedUser()
        {
            try
            {
                int codigoUsuario = int.Parse(User.FindFirst("CodigoUsuario").Value);
                var items = await service.GetByUser(codigoUsuario);
                return Ok(items);
            }
            catch (ArgumentException ex)
            {
                return StatusCode((int)HttpStatusCode.InternalServerError, ex.Message);
            }
        }

        [HttpPost]
        [Route("ImportFromExcel")]
        public async Task<ActionResult> ImportFromExcel([FromBody] ClienteImportacaoDto request)
        {
            try
            {
                await service.ImportarDadosClientes(request);
                return Ok();
            }
            catch (Exception ex)
            {
                return StatusCode((int)HttpStatusCode.InternalServerError, ex.Message);
            }

        }
    }
}
