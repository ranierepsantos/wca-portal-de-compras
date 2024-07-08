using System.ComponentModel.DataAnnotations;
using wca.compras.domain.Entities;

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
        int FilialId,
        decimal ValorFrete,
        decimal ValorCompraMinimoSemFrete,
        decimal TaxaGestaoMinimaPercentual,
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
        [Required(ErrorMessage = "O campo é obrigatório!")]
        int FilialId,
        decimal ValorFrete,
        decimal ValorCompraMinimoSemFrete,
        decimal TaxaGestaoMinimaPercentual,
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
        [Required(ErrorMessage = "O campo é obrigatório!")]
        int FilialId,
        decimal ValorFrete,
        decimal ValorCompraMinimoSemFrete,
        decimal TaxaGestaoMinimaPercentual,
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
        int TipoFornecimentoId,
        IList<ProdutoIcmsEstadoDto> ProdutoIcmsEstado
    );

    public record ProdutoWithIcmsDto(
        int Id,
        string Codigo,
        string Nome,
        string UnidadeMedida,
        decimal Valor,
        decimal TaxaGestao,
        decimal PercentualIPI,
        int FornecedorId,
        int TipoFornecimentoId,
        decimal Icms
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
        string Nome
    );

    public record ProdutoIcmsEstadoDto (string UF, decimal Icms);
}
