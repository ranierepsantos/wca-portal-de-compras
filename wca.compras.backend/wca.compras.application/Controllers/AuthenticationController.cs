using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using wca.compras.domain.Dtos;
using wca.compras.domain.Interfaces.Services;

namespace wca.compras.webapi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthenticationController : Controller
    {
        private readonly IAuthenticationService _service;
        public AuthenticationController(IAuthenticationService service)
        {
            _service = service;
        }

        /// <summary>
        /// Realiza a autenticação do usuário ao sistema
        /// </summary>
        /// <returns>Token</returns>
        /// <param name="login"></param>
        [AllowAnonymous]
        [HttpPost("Autenticar")]
        public async Task<ActionResult<LoginResponse>> Authenticate([FromBody] LoginRequest login)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest();
                }

                var result = await _service.Authenticate(login);

                return Ok(result);

            }
            catch (ArgumentException ex)
            {
                return StatusCode((int)HttpStatusCode.InternalServerError, ex.Message);
            }
        }

        /// <summary>
        /// Faz a recuperação de senha do usuário através de envio de email de recuperação
        /// </summary>
        /// <param name="model"></param>
        [AllowAnonymous]
        [HttpPost("RecuperarSenha")]
        public async Task<ActionResult> ForgotPassword([FromBody] ForgotPasswordRequest model)
        {
            if (!ModelState.IsValid || model == null)
            {
                return BadRequest();
            }

            try
            {
                await _service.ForgotPassword(model, Request.Headers["origin"]);
                return Ok();
            }
            catch (ArgumentException ex)
            {
                return StatusCode((int)HttpStatusCode.InternalServerError, ex.Message);
            }
        }

        /// <summary>
        /// Realiza a alteração da senha do usuário
        /// </summary>
        /// <param name="model"></param>
        [AllowAnonymous]
        [HttpPost("AlterarSenha")]
        public async Task<ActionResult> ResetPassword([FromBody] ResetPasswordRequest model)
        {
            if (!ModelState.IsValid || model == null)
            {
                return BadRequest();
            }

            try
            {
                await _service.ResetPassword(model);
                return Ok();
            }
            catch (ArgumentException ex)
            {
                return StatusCode((int)HttpStatusCode.InternalServerError, ex.Message);
            }
        }

    }
}
