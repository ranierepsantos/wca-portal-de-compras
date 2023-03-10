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
    public class FornecedorController : Controller
    {
        private IFornecedorService service;

        public FornecedorController(IFornecedorService service)
        {
            this.service = service;
        }

        /// <summary>
        /// Cadastra um novo Fornecedor
        /// </summary>
        /// <returns>NoContent</returns>
        /// <param name="createFornecedorDto"></param>
        [HttpPost]
        public async Task<ActionResult> Create([FromBody] CreateFornecedorDto createFornecedorDto)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest();
                }
                var result = await service.Create(createFornecedorDto);
                return Created("", result);
            }
            catch (ArgumentException ex)
            {
                return StatusCode((int)HttpStatusCode.InternalServerError, ex.Message);
            }
        }

        /// <summary>
        /// Atualiza dados do Fornecedor
        /// </summary>
        /// <returns>Fornecedor</returns>
        /// <param name="updateFornecedorDto"></param>
        [HttpPut]
        public async Task<ActionResult> Update([FromBody] UpdateFornecedorDto updateFornecedorDto)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest();
                }
                int filial = int.Parse(User.FindFirst("Filial").Value);

                var result = await service.Update(filial, updateFornecedorDto);
                if (result == null)
                {
                    return NotFound($"Fornecedor íd: {updateFornecedorDto.Id}, não localizado!");
                }
                return Ok(result);
            }
            catch (ArgumentException ex)
            {
                return StatusCode((int)HttpStatusCode.InternalServerError, ex.Message);
            }
        }

        /// <summary>
        /// Busca Fornecedor pelo Id
        /// </summary>
        /// <returns>Fornecedor</returns>
        /// <param name="id"></param>
        [HttpGet]
        [Route("{id}")]
        public async Task<ActionResult<FornecedorDto>> Get(int id)
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
                    return NotFound($"Fornecedor íd: {id}, não localizado!");
                }
                return Ok(result);
            }
            catch (ArgumentException ex)
            {
                return StatusCode((int)HttpStatusCode.InternalServerError, ex.Message);
            }
        }

        /// <summary>
        /// Retorna lista de Fornecedors ativos para preenchimento de Listas e Combos
        /// </summary>
        /// <returns>items</returns>
        /// <param name="filial"></param>
        [HttpGet]
        [Route("ToList/{filial}")]
        public async Task<ActionResult<IList<FornecedorListDto>>> List(int filial)
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
        /// Retorna lista de Fornecedors por paginação
        /// </summary>
        /// <returns>FornecedorDto</returns>
        [HttpGet]
        [Route("Paginate/{pageSize}/{page}")]
        public ActionResult<Pagination<FornecedorDto>> Paginate(int pageSize = 10, int page = 1, string? termo = "")
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
        /// Importar lista de Produtos do Fornecedor através de planilha
        /// </summary>
        [HttpPost]
        [Route("Produtos/ImportFromExcel")]
        public async Task<ActionResult> ProdutosImportFromExcel([FromBody] ImportProdutoDto importProdutoDto)
        {
            try
            {
                int filial = int.Parse(User.FindFirst("Filial").Value);
                await service.ImportProdutoFromExcel(filial, importProdutoDto.FornecedorId, importProdutoDto);
                return Ok();
            }
            catch (Exception ex)
            {
                return StatusCode((int)HttpStatusCode.InternalServerError, ex.Message);
            }
            
        }

        /// <summary>
        /// Retorna lista de Produtos do Fornecedor por paginação
        /// </summary>
        /// <returns>Paginnation</returns>
        [HttpGet]
        [Route("{fornecedorId}/Produtos/Paginate/{pageSize}/{page}")]
        public ActionResult<Pagination<FornecedorDto>> Paginate(int fornecedorId, int pageSize = 10, int page = 1, string? termo = "", bool onlyAuthUser = false)
        {
            try
            {
                int filial = int.Parse(User.FindFirst("Filial").Value);
                int idUsuario = 0;
                if (onlyAuthUser)
                {
                    idUsuario = int.Parse(User.FindFirst("CodigoUsuario").Value);
                }

                var items = service.Paginate(filial, fornecedorId, page, pageSize, termo, idUsuario);
                return Ok(items);
            }
            catch (Exception ex)
            {
                return StatusCode((int)HttpStatusCode.InternalServerError, ex.Message);
            }
        }

        /// <summary>
        /// Cadastra um novo Produto no Fornecedor
        /// </summary>
        /// <returns>NoContent</returns>
        /// <param name="createProdutoDto"></param>
        [HttpPost]
        [Route("Produto")]
        public async Task<ActionResult> CreateProduto([FromBody] CreateProdutoDto createProdutoDto)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest();
                }
                var result = await service.CreateProduto(createProdutoDto);
                return Created("", result);
            }
            catch (ArgumentException ex)
            {
                return StatusCode((int)HttpStatusCode.InternalServerError, ex.Message);
            }
        }

        /// <summary>
        /// Excluir Produto
        /// </summary>
        /// <returns>Fornecedor</returns>
        /// <param name="fornecedorId"></param>
        /// <param name="id"></param>
        [HttpDelete]
        [Route("Produto")]
        public async Task<ActionResult> RemoveProduto(int fornecedorId, int id)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest();
                }
                int filial = int.Parse(User.FindFirst("Filial").Value);

                var result = await service.RemoveProduto(filial, fornecedorId, id);
                if (!result)
                {
                    return NotFound();
                }
                return Ok(result);
            }
            catch (ArgumentException ex)
            {
                return StatusCode((int)HttpStatusCode.InternalServerError, ex.Message);
            }
        }

        /// <summary>
        /// Atualiza dados do Produto
        /// </summary>
        /// <returns>Fornecedor</returns>
        /// <param name="updateFornecedorDto"></param>
        [HttpPut]
        [Route("Produto")]
        public async Task<ActionResult> UpdateProduto([FromBody] UpdateProdutoDto updateProdutoDto)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest();
                }
                int filial = int.Parse(User.FindFirst("Filial").Value);

                var result = await service.UpdateProduto(filial, updateProdutoDto);
                if (result == null)
                {
                    return NotFound($"Produto íd: {updateProdutoDto.Id}, não localizado!");
                }
                return Ok(result);
            }
            catch (ArgumentException ex)
            {
                return StatusCode((int)HttpStatusCode.InternalServerError, ex.Message);
            }
        }

        /// <summary>
        /// Busca Produto do Fornecedor pelo Id
        /// </summary>
        /// <returns>Fornecedor</returns>
        /// <param name="fornecedorId"></param>
        /// <param name="id"></param>
        [HttpGet]
        [Route("{fornecedorId}/Produto/{id}")]
        public async Task<ActionResult<ProdutoDto>> GetProduto(int fornecedorId, int id)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest();
                }

                int filial = int.Parse(User.FindFirst("Filial").Value);

                var result = await service.GetProdutoById(filial,fornecedorId, id);
                if (result == null)
                {
                    return NotFound($"Produto íd: {id}, não localizado!");
                }
                return Ok(result);
            }
            catch (ArgumentException ex)
            {
                return StatusCode((int)HttpStatusCode.InternalServerError, ex.Message);
            }
        }

        [HttpGet]
        [Route("{fornecedorId}/Produtos/ExportExcel")]
        public async Task<ActionResult> ExportExcel(int fornecedorId)
        {
            try
            {
                Stream? st = await service.ProdutosExportToExcel(fornecedorId);

                return new FileStreamResult(st, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet") { FileDownloadName = $"Produtos_Fornecedor_{fornecedorId}.xlsx" };

            }
            catch (Exception ex)
            {
                return StatusCode((int)HttpStatusCode.InternalServerError, ex.Message);
            }

        }

    }
}
