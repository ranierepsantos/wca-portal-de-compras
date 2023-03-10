using System.ComponentModel.DataAnnotations;

namespace wca.compras.domain.Dtos
{
    public record FornecedorDto(
        int Id,
        string Nome,
        string CNPJ,
        string InscricaoEstadual,
        string Endereco,
        string Numero,
        string CEP,
        string Cidade,
        string UF,
        bool Ativo,
        decimal Icms,
        int FilialId,
        IList<FornecedorContatoDto> FornecedorContatos
    );

    public record CreateFornecedorDto(
        [Required(ErrorMessage = "O campo é obrigatório!")]
        string Nome,
        [Required(ErrorMessage = "O campo é obrigatório!")]
        string CNPJ,
        [Required(ErrorMessage = "O campo é obrigatório!")]
        string InscricaoEstadual,
        string Endereco,
        string Numero,
        string CEP,
        string Cidade,
        string UF,
        bool Ativo,
        decimal Icms,
        [Required(ErrorMessage = "O campo é obrigatório!")]
        int FilialId,
        [Required(ErrorMessage = "O campo é obrigatório!"), MinLength(1, ErrorMessage = "Não pode ser nulo ou vazio!")]
        IList<FornecedorContatoDto> FornecedorContatos
    );

    public record UpdateFornecedorDto(
        [Required(ErrorMessage = "O campo é obrigatório!")]
        int Id,
        [Required(ErrorMessage = "O campo é obrigatório!")]
        string Nome,
        [Required(ErrorMessage = "O campo é obrigatório!")]
        string CNPJ,
        [Required(ErrorMessage = "O campo é obrigatório!")]
        string InscricaoEstadual,
        string Endereco,
        string Numero,
        string CEP,
        string Cidade,
        string UF,
        bool Ativo,
        decimal Icms,
        [Required(ErrorMessage = "O campo é obrigatório!")]
        int FilialId,
        [Required(ErrorMessage = "O campo é obrigatório!"), MinLength(1, ErrorMessage = "Não pode ser nulo ou vazio!")]
        IList<FornecedorContatoDto> FornecedorContatos
    );

    public record ProdutoDto (
        int Id,
        string Codigo,
        string Nome,
        string UnidadeMedida,
        decimal Valor,
        decimal TaxaGestao,
        decimal PercentualIPI,
        int FornecedorId,
        int TipoFornecimentoId
    );

    public record CreateProdutoDto(
        [Required(ErrorMessage = "O campo é obrigatório!")] string Codigo,
        [Required(ErrorMessage = "O campo é obrigatório!")] string Nome,
        [Required(ErrorMessage = "O campo é obrigatório!")] string UnidadeMedida,
        [Required(ErrorMessage = "O campo é obrigatório!")] decimal Valor,
        decimal TaxaGestao,
        decimal PercentualIPI,
        [Required(ErrorMessage = "O campo é obrigatório!")] int FornecedorId,
        [Required(ErrorMessage = "O campo é obrigatório!")] int TipoFornecimentoId
    );

    public record UpdateProdutoDto(
        [Required(ErrorMessage = "O campo é obrigatório!")] int Id,
        [Required(ErrorMessage = "O campo é obrigatório!")] string Codigo,
        [Required(ErrorMessage = "O campo é obrigatório!")] string Nome,
        [Required(ErrorMessage = "O campo é obrigatório!")] string UnidadeMedida,
        [Required(ErrorMessage = "O campo é obrigatório!")] decimal Valor,
        decimal TaxaGestao,
        decimal PercentualIPI,
        [Required(ErrorMessage = "O campo é obrigatório!")] int FornecedorId,
        [Required(ErrorMessage = "O campo é obrigatório!")] int TipoFornecimentoId
    );

    public record ImportProdutoDto(
        [Required(ErrorMessage = "O campo é obrigatório!")] int FornecedorId,
        [Required(ErrorMessage = "O campo é obrigatório!")] string NomeArquivo,
        [Required(ErrorMessage = "O campo é obrigatório!")] string Arquivo
    );

    public record FornecedorContatoDto(
        int Id,
        int FornecedorId,
        [Required(ErrorMessage = "O campo é obrigatório!")]
        string Nome,
        [Required(ErrorMessage = "O campo é obrigatório!")]
        string Email,
        string Telefone,
        string Celular,
        bool AprovaPedido
    );

    public record FornecedorListDto (
        int Id,
        string Nome,
        decimal Icms
    );
}
