using System.ComponentModel.DataAnnotations;
using wca.compras.domain.Entities;
using wca.compras.domain.Util;

namespace wca.compras.domain.Dtos
{
    public record RecorrenciaDto (
        int Id,
        int FilialId,
        int ClienteId,
        int FornecedorId,
        int TipoRecorrencia,
        DateTime DataCriacao,
        int Dia,
        EnumDestinoRequisicao Destino,
        ListItem Cliente,
        ListItem Usuario,
        ListItem Fornecedor,
        IList<RecorrenciaProduto> RecorrenciaProdutos,
        IList<RecorrenciaLog> RecorrenciaLogs
    );

    public record CreateRecorrenciaDto(
        [Required(ErrorMessage = "O campo é obrigatório!")] int FilialId,
        [Required(ErrorMessage = "O campo é obrigatório!")] int ClienteId,
        [Required(ErrorMessage = "O campo é obrigatório!")] int FornecedorId,
        [Required(ErrorMessage = "O campo é obrigatório!")] int TipoRecorrencia,
        [Required(ErrorMessage = "O campo é obrigatório!")] int Dia,
        EnumDestinoRequisicao Destino,
        [Required(ErrorMessage = "Adicione pelo menos 1 produto"), MinLength(1, ErrorMessage = "Adicione pelo menos 1 produto")] IList<RecorrenciaProduto> RecorrenciaProdutos
    );
    public record UpdateRecorrenciaDto(
        [Required(ErrorMessage = "O campo é obrigatório!")] int Id,
        [Required(ErrorMessage = "O campo é obrigatório!")] int FilialId,
        [Required(ErrorMessage = "O campo é obrigatório!")] int ClienteId,
        [Required(ErrorMessage = "O campo é obrigatório!")] int FornecedorId,
        [Required(ErrorMessage = "O campo é obrigatório!")] int TipoRecorrencia,
        [Required(ErrorMessage = "O campo é obrigatório!")] int Dia,
        EnumDestinoRequisicao Destino,
        [Required(ErrorMessage = "Adicione pelo menos 1 produto"), MinLength(1, ErrorMessage = "Adicione pelo menos 1 produto")] IList<RecorrenciaProduto> RecorrenciaProdutos
    );

    public record RecorrenciaProdutoDto(
        int Id,
        int RequisicaoId,
        string Codigo,
        string Nome,
        string UnidadeMedida,
        int Quantidade
    );

    public record RecorrenciaLogDto(
        int Id,
        int RequisicaoId,
        DateTime DataHora,
        string Log
    );
}
