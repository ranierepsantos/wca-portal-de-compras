namespace wca.share.application.Features.Funcionarios.Common
{
    public record FuncionarioResponse (
        int Id,
        string Nome,
        int ClienteId,
        int CentroCustoId,
        DateTime DataAdmissao,
        int? CodigoFuncionario = null,
        DateTime? DataDemissao = null,
        string? Email = null,
        string? Cep = null,
        string? Endereco = null,
        string? Complemento = null,
        string? Bairro = null,
        string? Cidade = null,
        string? UF = null,
        int? DDDCelular = null,
        int? NumeroCelular = null
    );
    public sealed class FuncionarioToPaginateResponse
    {
        public int Id { get; set; }
        public string Nome { get; set; } = string.Empty;
        public string ClienteNome { get; set; } = string.Empty;
        public string CentroCustoNome { get; set; } = string.Empty;
        public int CodigoFuncionario { get; set; }
        public int? DDDCelular { get; set; }
        public int? NumeroCelular { get; set; }
    }

    public sealed class FuncionarioListItem
    {
        public int Value { get; set; }
        public string Text { get; set; } = string.Empty;
        public int CentroCustoId { get; set; }
        public string CentroCustoNome { get; set; } = string.Empty;
        public DateTime DataAdmissao { get; set; }
    }
}
