using Microsoft.AspNetCore.Mvc;
using System.Net;
using wca.compras.domain.Dtos;
using wca.compras.domain.Interfaces.Services;

namespace wca.compras.webapi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    //[Authorize("Bearer")]
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

                var result = await service.Update(1,clienteDto);
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
                var result = await service.GetById(1, id);
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

        [HttpGet]
        [Route("all")]
        public async Task<ActionResult<IList<ClienteDto>>> GetAll()
        {
            var items = await service.GetAll(1);
            return Ok(items);
        }


    }
}
