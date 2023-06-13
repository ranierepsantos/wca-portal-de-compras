using Microsoft.AspNetCore.Mvc;
using System.Net;
using wca.compras.domain.Interfaces.Services;
using wca.compras.domain.Util;
using wca.compras.domain.Dtos;
using Microsoft.AspNetCore.Authorization;

namespace wca.compras.webapi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    //[Authorize("Bearer")]
    public class PermissaoController : Controller
    {
        private IPermissaoService service;

        public PermissaoController(IPermissaoService service)
        {
            this.service = service;
        }

        [HttpGet]
        [Route("all/{sistemaId}")]
        public async Task<ActionResult<IList<PermissaoDto>>> GetAll(int sistemaId)
        {
            var items = await service.GetAll(sistemaId);
            return Ok(items);
        }

        [HttpGet]
        [Route("ToList/{sistemaId}")]
        public async Task<ActionResult<IList<ListItem>>> List(int sistemaId)
        {
            var items = await service.GetToList(sistemaId);
            return Ok(items);
        }
    }
}
