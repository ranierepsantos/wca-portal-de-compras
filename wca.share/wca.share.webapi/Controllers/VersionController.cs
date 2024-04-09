using Microsoft.AspNetCore.Mvc;

namespace wca.share.webapi.Controllers
{
    public class VersionController : ApiController
    {
        [HttpGet("GetVersion")]
        public IActionResult GetVersion()
        {
            return Ok("1.0.1");
        }
    }
}
