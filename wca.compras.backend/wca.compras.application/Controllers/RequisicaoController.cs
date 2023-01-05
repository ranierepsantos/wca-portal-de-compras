using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MiniExcelLibs;
using System.Net;
using wca.compras.domain.Dtos;
using wca.compras.domain.Entities;
using wca.compras.domain.Interfaces.Services;
using wca.compras.domain.Util;
using wca.compras.services;

namespace wca.compras.webapi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class RequisicaoController : Controller
    {
        private IRequisicaoService service;

        public RequisicaoController(IRequisicaoService service)
        {
            this.service = service;
        }

        //[HttpGet]
        //[Route("DownloadExcel")]
        //public async Task<ActionResult> DownloadExcel()
        //{
        //    var currentDirectory = Directory.GetCurrentDirectory();
        //    var parentDirectory = Directory.GetParent(currentDirectory).FullName;
        //    var filesDirectory = Path.Combine(parentDirectory, "Files");
        //    filesDirectory = Path.Combine(filesDirectory, "excel");

        //    var excelFile = Path.Combine(filesDirectory, "teste0.xlsx");
        //    var saveFile = Path.Combine(filesDirectory, $"teste0exported.xlsx");

        //    // 1. By POCO
        //    // var tvalue = new
        //    // {
        //    //     title = "FooCompany",
        //    //     managers = new[] {
        //    //                     new {name="Jack",department="HR"},
        //    //                     new {name="Loan",department="IT"}
        //    //                 },
        //    //     employees = new[] {
        //    //                         new {name="Wade",department="HR"},
        //    //                         new {name="Felix",department="HR"},
        //    //                         new {name="Eric",department="IT"},
        //    //                         new {name="Keaton",department="IT"}
        //    //                     }
        //    // };
        //    var tvalue = new
        //    {
        //        employees = new[] {
        //                        new {name="Jack",department="HR"},
        //                        new {name="Lisa",department="HR"},
        //                        new {name="John",department="HR"},
        //                        new {name="Mike",department="IT"},
        //                        new {name="Neo",department="IT"},
        //                        new {name="Loan",department="IT"}
        //                    }
        //    };
        //    MiniExcel.SaveAsByTemplate(saveFile, excelFile, tvalue);

        //    // var requisicao = await service.GetById(1, 12);

        //    // var value = new
        //    // {
        //    //     pedido = requisicao.Id,
        //    //     cliente = requisicao.Cliente.Nome,
        //    //     cnpj = requisicao.Cliente.CNPJ,
        //    //     endereco = "",
        //    //     supervisor = requisicao.Usuario.Text,
        //    //     Produtos = requisicao.RequisicaoItens,
        //    // };

        //    // MiniExcel.SaveAsByTemplate(saveFile, excelFile, value);
        //    return Ok();


        //    // var values = new[] {
        //    //                     new { Name = "MiniExcel", Valor = 1 },
        //    //                     new { Name = "Github", Valor = 2}
        //    //                 };

        //    // var memoryStream = new MemoryStream();
        //    // memoryStream.SaveAs(values);
        //    // memoryStream.Seek(0, SeekOrigin.Begin);
        //    // return new FileStreamResult(memoryStream, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet")
        //    // {
        //    //     FileDownloadName = "demo.xlsx"
        //    // };
        //}

        /// <summary>
        /// Cadastra uma nova Requisição
        /// </summary>
        /// <returns>NoContent</returns>
        /// <param name="createRequisicaoDto"></param>
        [HttpPost]
        [Authorize("Bearer")]
        public async Task<ActionResult> Create([FromBody] CreateRequisicaoDto createRequisicaoDto)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest();
                }

                var result = await service.Create(createRequisicaoDto, Request.Headers["origin"]);

                return Created("", result);
            }
            catch (ArgumentException ex)
            {
                return StatusCode((int)HttpStatusCode.InternalServerError, ex.Message);
            }
        }

        /// <summary>
        /// Aprova/Rejeita uma Requisição
        /// </summary>
        /// <returns>NoContent</returns>
        /// <param name="aprovarRequisicaoDto"></param>
        [HttpPost]
        [Route("AprovarReprovar")]
        public async Task<ActionResult> AprovarReprovar([FromBody] AprovarRequisicaoDto aprovarRequisicaoDto)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest();
                }

                var result = await service.aprovarRequisicao(aprovarRequisicaoDto);

                if (result == false)
                    return NotFound();


                return NoContent();
            }
            catch (ArgumentException ex)
            {
                return StatusCode((int)HttpStatusCode.InternalServerError, ex.Message);
            }
        }


        /// <summary>
        /// Atualiza dados da Requisição
        /// </summary>
        /// <returns>NoContent</returns>
        /// <param name="updateRequisicaoDto"></param>
        [HttpPut]
        [Authorize("Bearer")]
        public async Task<ActionResult> Update([FromBody] UpdateRequisicaoDto updateRequisicaoDto)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest();
                }

                int filial = int.Parse(User.FindFirst("Filial").Value);

                var result = await service.Update(filial, updateRequisicaoDto);
                if (result == null)
                {
                    return NotFound($"Requisição íd: {updateRequisicaoDto.Id}, não localizado!");
                }
                return Ok(result);
            }
            catch (ArgumentException ex)
            {
                return StatusCode((int)HttpStatusCode.InternalServerError, ex.Message);
            }
        }

        /// <summary>
        /// Busca Requisição pelo Id
        /// </summary>
        /// <returns>RequisicaoDto</returns>
        /// <param name="id"></param>
        [HttpGet]
        [Route("{id}")]
        [Authorize("Bearer")]
        public async Task<ActionResult<RequisicaoDto>> Get(int id)
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
                    return NotFound($"Requisição íd: {id}, não localizado!");
                }
                return Ok(result);
            }
            catch (ArgumentException ex)
            {
                return StatusCode((int)HttpStatusCode.InternalServerError, ex.Message);
            }
        }

        /// <summary>
        /// Busca Requisição pelo token para aprovação/rejeição
        /// </summary>
        /// <returns>RequisicaoDto</returns>
        /// <param name="token"></param>
        [HttpGet]
        [Route("GetByToken/{token}")]
        public async Task<ActionResult<RequisicaoDto>> GetByToken(string token)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest();
                }

                var result = await service.GetByAprovacaoToken(token);
                if (result == null)
                {
                    return NotFound($"Requisição, não localizado!");
                }
                return Ok(result);
            }
            catch (ArgumentException ex)
            {
                return StatusCode((int)HttpStatusCode.InternalServerError, ex.Message);
            }
        }


        /// <summary>
        /// Retorna lista de Requisição por paginação
        /// </summary>
        /// <returns>RequisicaoDto</returns>
        [HttpGet]
        [Route("Paginate/{pageSize}/{page}")]
        [Authorize("Bearer")]
        public ActionResult<Pagination<RequisicaoDto>> Paginate(int pageSize = 10, int page = 1, int clienteId = 0, int fornecedorId = 0, int usuarioId = 0, EnumStatusRequisicao status = EnumStatusRequisicao.TODOS)
        {
            try
            {
                int filial = int.Parse(User.FindFirst("Filial").Value);
                var items = service.Paginate(filial, page, pageSize, clienteId, fornecedorId, usuarioId, status);
                return Ok(items);
            }
            catch (Exception ex)
            {
                return StatusCode((int)HttpStatusCode.InternalServerError, ex.Message);
            }

        }

        /// <summary>
        /// Altera o status da requisição para Cancelado
        /// </summary>
        /// <param name="id"></param>
        [HttpDelete]
        [Authorize("Bearer")]
        public async Task<ActionResult> Remove(int id)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest();
                }
                int filial = int.Parse(User.FindFirst("Filial").Value);
                string nomeUsuario = User.FindFirst("UsuarioNome").Value;
                var result = await service.Remove(filial, id, nomeUsuario);

                if (!result) return NotFound($"Requisição, não localizado!");

                return Ok(result);
            }
            catch (Exception ex)
            {
                return StatusCode((int)HttpStatusCode.InternalServerError, ex.Message);
            }
        }
    }
}
