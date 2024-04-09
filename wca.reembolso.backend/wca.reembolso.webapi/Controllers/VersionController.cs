using Microsoft.AspNetCore.Mvc;

namespace wca.reembolso.webapi.Controllers
{
    public class VersionController : ApiController
    {
        [HttpGet("GetVersion")]
        public IActionResult GetVersion()
        {
            return Ok("1.0.16");
        }
    }
}
