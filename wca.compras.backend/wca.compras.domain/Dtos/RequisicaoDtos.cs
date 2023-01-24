using System.ComponentModel.DataAnnotations;
using wca.compras.domain.Entities;
using wca.compras.domain.Util;

namespace wca.compras.domain.Dtos
{
    public record RequisicaoDto(
        int Id,
        int FilialId,
        int ClienteId,
        int FornecedorId,
        DateTime DataCriacao,
        decimal ValorTotal,
        decimal TaxaGestao,
        EnumStatusRequisicao Status,
        EnumDestinoRequisicao Destino,
        string NotaFiscal,
        DateTime DataEntrega,
        Cliente Cliente,
        ListItem Usuario,
        ListItem Fornecedor,
        IList<RequisicaoHistoricoDto> RequisicaoHistorico,
        IList<RequisicaoItemDto> RequisicaoItens
    );

    public record RequisicaoAprovacaoDto(
        int Id,
        decimal ValorTotal,
        decimal TaxaGestao,
        EnumDestinoRequisicao Destino,
        Cliente Cliente,
        ListItem Usuario,
        IList<RequisicaoItemDto> RequisicaoItens,
        string NomeAprovador = ""
    );

    public record AprovarRequisicaoDto(
        int Id,
        bool Aprovado,
        string Comentario,
        string Token
    );

    public record FinalizarRequisicaoDto(
        int Id,
        string NotaFiscal,
        DateTime DataEntrega
    );

    public record CreateRequisicaoDto (
        [Required(ErrorMessage = "O campo é obrigatório!")] int FilialId,
        [Required(ErrorMessage = "O campo é obrigatório!")] int ClienteId,
        [Required(ErrorMessage = "O campo é obrigatório!")] int FornecedorId,
        [Required(ErrorMessage = "O campo é obrigatório!")] decimal ValorTotal,
        decimal TaxaGestao,
        EnumDestinoRequisicao Destino,
        [Required(ErrorMessage = "O campo é obrigatório!")] int UsuarioId,
        [Required(ErrorMessage = "O campo é obrigatório!")] string NomeUsuario,
        [Required(ErrorMessage = "Adicione pelo menos 1 produto"), MinLength(1, ErrorMessage = "Adicione pelo menos 1 produto")] IList<RequisicaoItemDto> RequisicaoItens,
        bool RequerAutorizacaoWCA = false,
        bool RequerAutorizacaoCliente = false
    );

    public record UpdateRequisicaoDto(
        [Required(ErrorMessage = "O campo é obrigatório!")] int Id,
        [Required(ErrorMessage = "O campo é obrigatório!")] int FilialId,
        [Required(ErrorMessage = "O campo é obrigatório!")] int ClienteId,
        [Required(ErrorMessage = "O campo é obrigatório!")] int FornecedorId,
        [Required(ErrorMessage = "O campo é obrigatório!")] decimal ValorTotal,
        decimal TaxaGestao,
        EnumStatusRequisicao Status,
        EnumDestinoRequisicao Destino,
        [Required(ErrorMessage = "O campo é obrigatório!")] string NomeUsuario,
        IList<RequisicaoItemDto> RequisicaoItens,
        IList<RequisicaoHistoricoDto> RequisicaoHistorico,
        bool RequerAutorizacaoWCA = false,
        bool RequerAutorizacaoCliente = false
    );

    public record RequisicaoItemDto (
        int Id,
        int RequisicaoId,
        string Codigo,
        string Nome,
        string UnidadeMedida,
        decimal Valor,
        decimal TaxaGestao,
        int Quantidade,
        decimal ValorTotal,
        int TipoFornecimentoId
    );

    public record RequisicaoHistoricoDto(
        int Id,
        int RequisicaoId,
        DateTime DataHora,
        string Evento
    );

    public class RequisicaoCategoriaConsolidadoDto
    {
        public int TipoFornecimentoId { get; set; }
        public int Quantidade { get; set; }
    }

}