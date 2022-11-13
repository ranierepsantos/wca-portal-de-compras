using Microsoft.AspNetCore.Mvc;
using wca.compras.domain.Interfaces.Services;
using wca.compras.domain.Util;
using static wca.compras.domain.Dtos.PermissionDtos;

namespace wca.compras.webapi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class PermissionController : Controller
    {
        private IPermissionService service;

        public PermissionController(IPermissionService service)
        {
            this.service = service;
        }

        [HttpGet]
        [Route("all")]
        public async Task<ActionResult<IList<PermissionDto>>> GetAll()
        {
            var items = await service.GetAll();
            return Ok(items);
        }

        [HttpGet]
        [Route("ToList")]
        public async Task<ActionResult<IList<ListItem>>> List()
        {
            var items = await service.GetToList();
            return Ok(items);
        }

        [HttpPost]
        public async Task<ActionResult<PermissionDto>> Create(CreatePermissionDto createPermission)
        {
            var permission = await service.Create(createPermission);
            return Ok(permission);
        }

        //[HttpGet]
        //public async Task<ActionResult<IList<ListItem>>> GetToList()
        //{
        //    var list = await service.GetToList();
        //    return Ok(list);
        //}
    }
}
