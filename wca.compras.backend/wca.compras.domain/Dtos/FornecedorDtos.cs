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
        int FilialId
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
        [Required(ErrorMessage = "O campo é obrigatório!")]
        int FilialId
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
        [Required(ErrorMessage = "O campo é obrigatório!")]
        int FilialId
    );

    public record ProdutoDto (
        int Id,
        string Codigo,
        string Nome,
        string UnidadeMedida,
        decimal Valor,
        decimal TaxaGestao,
        int FornecedorId,
        int TipoFornecimentoId
    );

    public record CreateProdutoDto(
        [Required(ErrorMessage = "O campo é obrigatório!")] string Codigo,
        [Required(ErrorMessage = "O campo é obrigatório!")] string Nome,
        [Required(ErrorMessage = "O campo é obrigatório!")] string UnidadeMedida,
        [Required(ErrorMessage = "O campo é obrigatório!")] decimal Valor,
        decimal TaxaGestao,
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
        [Required(ErrorMessage = "O campo é obrigatório!")] int FornecedorId,
        [Required(ErrorMessage = "O campo é obrigatório!")] int TipoFornecimentoId
    );

    public record ImportProdutoDto(
        [Required(ErrorMessage = "O campo é obrigatório!")] int FornecedorId,
        [Required(ErrorMessage = "O campo é obrigatório!")] string NomeArquivo,
        [Required(ErrorMessage = "O campo é obrigatório!")] string Arquivo
    );
}
