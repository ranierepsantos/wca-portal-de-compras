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
    public class UsuarioController : Controller
    {
        private readonly IUsuarioService _usuarioService;
        private readonly IAuthenticationService _authenticationService;

        public UsuarioController(IUsuarioService userService, IAuthenticationService authenticationService)
        {
            _usuarioService = userService;
            _authenticationService = authenticationService;
        }

        /// <summary>
        /// Cadastra um novo usuário
        /// </summary>
        /// <returns>Usuario</returns>
        /// <param name="createUsuario"></param>
        [HttpPost]
        public async Task<ActionResult> Add([FromBody] CreateUsuarioDto createUsuario)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest();
                }

                var result = await _usuarioService.Create(createUsuario);

                //await _authenticationService.ForgotPassword(new ForgotPasswordRequest(createUsuario.Email), Request.Headers["origin"]);
                
                return Created("", result);
            }
            catch (Exception ex)
            {
                return StatusCode((int)HttpStatusCode.InternalServerError, ex.Message);
            }
        }


        /// <summary>
        /// Atualiza informações do Usuário
        /// </summary>
        /// <returns>Usuario</returns>
        /// <param name="updateUsuario"></param>
        [HttpPut]
        public async Task<ActionResult> Update([FromBody] UpdateUsuarioDto updateUsuario)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest();
                }
                int filial = int.Parse(User.FindFirst("Filial").Value);
                var result = await _usuarioService.Update(filial, updateUsuario);
                if (result == null) return NotFound();
                return NoContent();
            }
            catch (Exception ex)
            {
                return StatusCode((int)HttpStatusCode.InternalServerError, ex.Message);
            }
        }

        /// <summary>
        /// Excluir Usuário
        /// </summary>
        /// <param name="id"></param>
        [HttpDelete]
        public async Task<ActionResult> Remove(int id)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest();
                }
                int filial = int.Parse(User.FindFirst("Filial").Value);
                var result = await _usuarioService.Remove(filial, id);
                
                if (!result) return NotFound();

                return Ok(result);
            }
            catch (Exception ex)
            {
                return StatusCode((int)HttpStatusCode.InternalServerError, ex.Message);
            }
        }

        /// <summary>
        /// Retorna Usuario por paginação
        /// </summary>
        /// <returns>Perfil</returns>
        [HttpGet]
        [Route("Paginate/{pageSize}/{page}")]
        public async Task<ActionResult<Pagination<UsuarioDto>>> Paginate(int pageSize = 10, int page = 1, string? termo = "")
        {
            
            try
            {
                int filial = int.Parse(User.FindFirst("Filial").Value);
                var items = await _usuarioService.Paginate(filial, page, pageSize, termo);
                return Ok(items);
            }
            catch (Exception ex)
            {
                return StatusCode((int)HttpStatusCode.InternalServerError, ex.Message);
            }
            
        }
    }
}
