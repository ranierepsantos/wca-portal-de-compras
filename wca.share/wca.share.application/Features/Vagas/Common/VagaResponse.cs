using wca.share.application.Common;
using wca.share.application.Features.Solicitacoes.Common;
using wca.share.domain.Entities;

namespace wca.share.application.Features.Vagas.Common
{
    public sealed class VagaResponse
    {
        public int Id { get; set; }
        public int ClienteId { get; set; }
        public string? ClienteNome { get; set; }
        public int TipoContratoId { get; set; }
        public string? TipoContratoNome { get; set; }
        public int TipoFaturamentoId { get; set; }
        public string? TipoFaturamentoNome { get; set; }
        public int GestorId { get; set; }
        public string? GestorNome { get; set; }
        public int FuncaoId { get; set; }
        public string? FuncaoNome { get; set; }
        public int EscalaId { get; set; }
        public string? EscalaNome { get; set; }
        public int HorarioId { get; set; }
        public string? HorarioNome { get; set; }
        public DateTime DataSolicitacao { get; set; }
        public int StatusSolicitacaoId { get; set; }
        public string? StatusSolicitacaoNome { get; set; }
        public int QuantidadeVagas { get; set; }
        public int SexoId { get; set; }
        public string? SexoNome { get; set; }
        public int IdadeMinima { get; set; }
        public int IdadeMaxima { get; set; }
        public string? Caracteristica { get; set; }
        public string? Indicacao { get; set; }
        public string? EnderecoCliente { get; set; }
        public string? Anotacoes { get; set; }
        public int MotivoContratacaoId { get; set; }
        public string? MotivoContratacaoNome { get; set; }
        public string? JustificativaContratacao { get; set; }
        public bool PermiteFumante { get; set; } = false;
        public int EscolaridadeId { get; set; }
        public string? EscolaridadeNome { get; set; }
        public string? LocalResidencia { get; set; }
        public string? ExperienciaProfissinal { get; set; }
        public string? DescricaoAtividades { get; set; }
        public bool TemCNH { get; set; } = false;
        public string? CategoriaCNH { get; set; }
        public bool TemValeTransporte { get; set; } = false;
        public decimal? ValorValeTransporte { get; set; }
        public int? DiasValeTransporte { get; set; }
        public string? Refeicao { get; set; }
        public string? OutrosBeneficios { get; set; }
        public decimal? SalarioBase { get; set; }
        public bool TemInsalubridade { get; set; } = false;
        public decimal PercentualInsalubridade { get; set; }
        public bool TemPericulosidade { get; set; } = false;
        public decimal PercentualPericulosidade { get; set; }
        public DateTime DataInicioPrevista { get; set; }
        public bool TemCopiaAdmissaoCliente { get; set; } = false;
        public bool TemIntegracaoCliente { get; set; } = false;
        public string? HorarioIntegracao { get; set; }
        public string? IntegracaoDiasSemana { get; set; }
        public List<ListItem>? DocumentoComplementares { get; set; }
        public List<VagaHistorico>? VagaHistorico { get; set; }

    }

    public sealed class VagaToPaginateResponse
    {
        public int Id { get; set; }
        public int FilialId { get; set; }
        public int Status { get; set; }
        public DateTime DataSolicitacao { get; set; }
        public string ClienteNome { get; set; }
        public string? ResponsavelNome { get; set; }
        public string? FuncaoNome { get; set; }

        public StatusSolicitacaoResponse StatusSolicitacao { get; set; }
        public List<VagaHistorico> VagaHistorico { get; set; }
    }

    public sealed class StatusSolicitacaoResponse
    {
        public int Id { get; set; }

        public string Status { get; set; }

        public string StatusIntermediario { get; set; }

        public string Color { get; set; }

    }

}
