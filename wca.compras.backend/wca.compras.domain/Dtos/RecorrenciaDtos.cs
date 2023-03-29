using System.ComponentModel.DataAnnotations;
using wca.compras.domain.Entities;
using wca.compras.domain.Util;

namespace wca.compras.domain.Dtos
{
    public record RecorrenciaDto (
        int Id,
        string Nome,
        int FilialId,
        int ClienteId,
        int FornecedorId,
        int UsuarioId,
        int TipoRecorrencia,
        DateTime DataCriacao,
        int Dia,
        EnumDestinoRequisicao Destino,
        bool Ativo,
        ListItem Cliente,
        ListItem Usuario,
        ListItem Fornecedor,
        IList<RecorrenciaProdutoDto> RecorrenciaProdutos,
        IList<RecorrenciaLogDto> RecorrenciaLogs,
        string PeriodoEntrega,
        string Endereco,
        string Numero,
        string CEP,
        string Cidade,
        string UF
    );

    public record EnabledRecorrenciaDto (
        int Id,
        bool Ativo
    );
    public record CreateRecorrenciaDto(
        [Required(ErrorMessage = "O campo é obrigatório!")] string Nome,
        [Required(ErrorMessage = "O campo é obrigatório!")] int FilialId,
        [Required(ErrorMessage = "O campo é obrigatório!")] int ClienteId,
        [Required(ErrorMessage = "O campo é obrigatório!")] int FornecedorId,
        [Required(ErrorMessage = "O campo é obrigatório!")] int UsuarioId,
        [Required(ErrorMessage = "O campo é obrigatório!")] int TipoRecorrencia,
        [Required(ErrorMessage = "O campo é obrigatório!")] int Dia,
        EnumDestinoRequisicao Destino,
        [Required(ErrorMessage = "Adicione pelo menos 1 produto"), MinLength(1, ErrorMessage = "Adicione pelo menos 1 produto")] IList<RecorrenciaProdutoDto> RecorrenciaProdutos,
        [Required(ErrorMessage = "O campo é obrigatório!")] string Endereco,
        [Required(ErrorMessage = "O campo é obrigatório!")] string Numero,
        [Required(ErrorMessage = "O campo é obrigatório!")] string CEP,
        [Required(ErrorMessage = "O campo é obrigatório!")] string Cidade,
        [Required(ErrorMessage = "O campo é obrigatório!")] string UF,
        string PeriodoEntrega = ""
    );
    public record UpdateRecorrenciaDto(
        [Required(ErrorMessage = "O campo é obrigatório!")] int Id,
        [Required(ErrorMessage = "O campo é obrigatório!")] string Nome,
        [Required(ErrorMessage = "O campo é obrigatório!")] int FilialId,
        [Required(ErrorMessage = "O campo é obrigatório!")] int ClienteId,
        [Required(ErrorMessage = "O campo é obrigatório!")] int FornecedorId,
        [Required(ErrorMessage = "O campo é obrigatório!")] int TipoRecorrencia,
        [Required(ErrorMessage = "O campo é obrigatório!")] int Dia,
        bool Ativo,
        EnumDestinoRequisicao Destino,
        [Required(ErrorMessage = "Adicione pelo menos 1 produto"), MinLength(1, ErrorMessage = "Adicione pelo menos 1 produto")] IList<RecorrenciaProdutoDto> RecorrenciaProdutos,
        [Required(ErrorMessage = "O campo é obrigatório!")] string Endereco,
        [Required(ErrorMessage = "O campo é obrigatório!")] string Numero,
        [Required(ErrorMessage = "O campo é obrigatório!")] string CEP,
        [Required(ErrorMessage = "O campo é obrigatório!")] string Cidade,
        [Required(ErrorMessage = "O campo é obrigatório!")] string UF,
        string PeriodoEntrega = ""
    );
    
    public record RecorrenciaProdutoDto(
        int Id ,
        int RecorrenciaId ,
        string Codigo ,
        string Nome ,
        string UnidadeMedida ,
        int Quantidade
    );

    
    public record RecorrenciaLogDto(
        int Id,
        int RecorrenciaId ,
        DateTime DataHora,
        string Status ,
        string Log 
    );
}
