using MediatR;
using Microsoft.AspNetCore.Mvc;
using wca.reembolso.application.Features.Despesas.Command;
using wca.reembolso.application.Features.Despesas.Queries;
using wca.reembolso.application.Features.SolicitacaoHistoricos.Commands;
using wca.reembolso.application.Features.Solicitacaos.Queries;
using wca.reembolso.application.Features.Solicitacoes.Commands;
using wca.reembolso.application.Features.Solicitacoes.Queries;

namespace wca.reembolso.webapi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    
    public class SolicitacaoController : ApiController
    {
        private readonly IMediator _mediator;

        public SolicitacaoController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet]
        public async Task<IActionResult> GetById([FromQuery] SolicitacaoByIdQuerie command) 
        {
            var result = await _mediator.Send(command);
        
            if (result.IsError) { return Problem(result.Errors); }

            return Ok(result.Value);
        }

        [HttpPost]
        public async Task<IActionResult> Create(SolicitacaoCreateCommand createCommand)
        {
            var result = await _mediator.Send(createCommand);

            if (result.IsError) { return Problem(result.Errors); }

            return Ok(result.Value);
        }

        [HttpGet("Paginar")]
        public async Task<IActionResult> ToPaginate([FromQuery] SolicitacaoPaginateQuery querie)
        {
            var result = await _mediator.Send(querie);

            if (result.IsError) { return Problem(result.Errors); }

            return Ok(result.Value);
        }

        [HttpPut]
        public async Task<IActionResult> Update(SolicitacaoUpdateCommand updateCommand)
        {
            var result = await _mediator.Send(updateCommand);

            if (result.IsError) { return Problem(result.Errors); }

            return Ok(result.Value);
        }

        [HttpGet("ListarPorColaborador")]
        public async Task<IActionResult> ListarPorColaborador([FromQuery] SolicitacaoByColaboradorQuery querie)
        {
            var result = await _mediator.Send(querie);

            if (result.IsError) { return Problem(result.Errors); }

            return Ok(result.Value);
        }

        [HttpGet("ListarStatusSolicitacao")]
        public async Task<IActionResult> ListarStatusSolicitacao()
        {
            var result = await _mediator.Send(new StatusSolicitacaoGetAllQuery());

            if (result.IsError) { return Problem(result.Errors); }

            return Ok(result.Value);

        }

        [HttpPut("AlterarStatus")]
        public async Task<IActionResult> AlterarStatus([FromBody] SolicitacaoChangeStatusCommand command)
        {
            var result = await _mediator.Send(command);

            if (result.IsError) { return Problem(result.Errors); }

            return Ok(result.Value);

        }

        
        [HttpPost("RegistrarEvento")]
        public async Task<IActionResult> RegistrarEvento([FromBody] SolicitacaoHistorioCreateCommand command)
        {
            var result = await _mediator.Send(command);

            if (result.IsError) { return Problem(result.Errors); }

            return Ok(result.Value);

        }

        [HttpGet("ChecarSeDespesaExiste")]
        public async Task<IActionResult> DespesaCheckIfExists([FromQuery] DespesaChecarByNotaAndCnpjQuery query)
        {
            var result = await _mediator.Send(query);

            if (result.IsError) { return Problem(result.Errors); }

            return Ok(result.Value);

        }

        [HttpGet("ExportarParaExcel")]
        public async Task<IActionResult> Export2Excel([FromQuery] SolicitacaoExportToExcelQuery querie)
        {
            var result = await _mediator.Send(querie);

            if (result.IsError) { return Problem(result.Errors); }

            return new FileStreamResult(result.Value, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet") { FileDownloadName = $"relatorio_solicitacoes.xlsx" };

        }

        [HttpPost("Despesa")]
        public async Task<IActionResult> CreateDespesa([FromBody] DespesaCreateCommand command)
        {
            var result = await _mediator.Send(command);

            if (result.IsError) { return Problem(result.Errors); }

            return Ok(result.Value);

        }

        [HttpPut("Despesa")]
        public async Task<IActionResult> UpdateDespesa([FromBody] DespesaUpdateCommand command)
        {
            var result = await _mediator.Send(command);

            if (result.IsError) { return Problem(result.Errors); }

            return Ok(result.Value);

        }
        [HttpDelete("Despesa/{Id}")]
        public async Task<IActionResult> DeleteDespesa([FromRoute] DespesaDeleteCommand command)
        {
            var result = await _mediator.Send(command);

            if (result.IsError) { return Problem(result.Errors); }

            return Ok(result.Value);

        }

        [HttpGet("ChecarVencidos")]
        public async Task<IActionResult> ChecarVencidos()
        {
            var result = await _mediator.Send(new SolicitacaoCheckVencidoCommand());

            if (result.IsError) { return Problem(result.Errors); }

            return Ok(result.Value);

        }

    }
}
