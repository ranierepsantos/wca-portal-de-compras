using Microsoft.AspNetCore.Mvc;
using System.Net;
using wca.compras.domain.Interfaces.Services;
using static wca.compras.domain.Dtos.PerfilDtos;

namespace wca.compras.webapi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ProfileController : Controller
    {
        private readonly IProfileService _service;
        public ProfileController(IProfileService profileService)
        {
            _service = profileService;
        }

        [HttpPost]
        public async Task<ActionResult> Add([FromBody] CreateProfileDto profileDto)
        {
            try
            {
                var result = await _service.Create(profileDto);
                return Created("", result);
            }
            catch (ArgumentException ex)
            {
                return StatusCode((int)HttpStatusCode.InternalServerError, ex.Message);
            }
        }


        [HttpGet]
        [Route("ProfileWithPermissions")]
        public async Task<ActionResult<ProfileDto>> GetProfileWithPermissions(string id)
        {
            try
            {
                var result = await _service.GetProfilePermissions(id);
                return Ok(result);
            }
            catch (ArgumentException ex)
            {
                return StatusCode((int)HttpStatusCode.InternalServerError, ex.Message);
            }
        }
    }
}
