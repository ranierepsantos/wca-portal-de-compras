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
        ListItem Cliente,
        ListItem Usuario,
        ListItem Fornecedor,
        IList<RequisicaoHistoricoDto> RequisicaoHistorico,
        IList<RequisicaoItemDto> RequisicaoItens
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
        [Required(ErrorMessage = "Adicione pelo menos 1 produto"), MinLength(1, ErrorMessage = "Adicione pelo menos 1 produto")] IList<RequisicaoItemDto> RequisicaoItens

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
        IList<RequisicaoHistoricoDto> RequisicaoHistorico
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

}