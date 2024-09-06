using wca.share.application.Common;
using wca.share.domain.Entities;

namespace wca.share.application.Features.Solicitacoes.Common
{
    public sealed class SolicitacaoResponse
    {
        public int Id { get; set; }
        public int SolicitacaoTipoId { get; set; }
        public int ClienteId { get; set; }
        public string ClienteNome { get; set; }
        public int? ResponsavelId { get; set; }
        public string? ResponsavelNome { get; set; }
        public DateTime DataSolicitacao { get;  set; }
        public string Descricao { get;  set; }
        public int StatusSolicitacaoId { get;  set; }
        public SolicitacaoComunicadoPaginateResponse? Comunicado { get;  set; }
        public SolicitacaoDesligamentoResponse? Desligamento { get;  set; }
        public SolicitacaoFeriasResponse? Ferias { get;  set; }
        public SolicitacaoMudancaBaseResponse? MudancaBase { get;  set; }
        public SolicitacaoVagaResponse? Vaga { get; set; }
        public List<SolicitacaoArquivo>? Anexos { get;  set; }
        public List<SolicitacaoHistorico> Historico { get; set; }
    }

    public sealed class SolicitacaoToPaginateResponse {
        public int Id { get; set; }
        public int FilialId { get; set; }
        public int SolicitacaoTipoId { get; set; }
        public string? Tipo { get; set; }
        public int Status { get; set; }
        public DateTime DataSolicitacao { get; set; }
        public string ClienteNome { get; set; }
        public string? ResponsavelNome { get; set; }
        public SolicitacaoComunicadoPaginateResponse? Comunicado { get; set; }
        public SolicitacaoFuncionarioCentroCustoPaginateResponse? Desligamento { get; set; }
        public SolicitacaoFuncionarioCentroCustoPaginateResponse? Ferias { get; set; }
        public SolicitacaoFuncionarioCentroCustoPaginateResponse? MudancaBase { get; set; }
        public SolicitacaoVagaPaginateResponse? Vaga { get; set; }   
        public StatusSolicitacaoResponse StatusSolicitacao { get; set; }
        public List<SolicitacaoHistorico> Historico { get; set; }
    }

    public sealed class  StatusSolicitacaoResponse
    {
        public int Id { get; set; }

        public string Status { get; set; } 

        public string StatusIntermediario { get; set; }

        public string Color { get; set; }

    }

    public class SolicitacaoFuncionarioCentroCustoPaginateResponse
    {
        public string? FuncionarioNome { get; init; }
        public string? CentroCustoNome { get; set; }
    }

    public class SolicitacaoComunicadoPaginateResponse : SolicitacaoFuncionarioCentroCustoPaginateResponse
    {
        public string? AssuntoNome { get; set; }
    }

    public sealed class SolicitacaoVagaPaginateResponse
    {
        public string? FuncaoNome { get; set; }
    }

    public sealed class SolicitacaoComunicadoResponse
    {
        public int SolicitacaoId { get; init; }
        public int FuncionarioId { get; set; }
        public string? FuncionarioNome { get; init; }
        public DateTime FuncionarioDataAdmissao { get; init; }
        public string? eSocialMatricula { get; set; }
        public int? CentroCustoId { get; set; }
        public string? CentroCustoNome { get; set; }
        public int AssuntoId { get; set; }
        public string? AssuntoNome { get; set; }
        public DateTime? DataAlteracao { get; set; }
        public string? Observacao { get; set; } = string.Empty;
    }

    public sealed class SolicitacaoDesligamentoResponse
    {
        public int SolicitacaoId { get; init; }
        public int FuncionarioId { get; set; }
        public string? FuncionarioNome { get; init; }
        public DateTime FuncionarioDataAdmissao { get; init; }
        public string? eSocialMatricula { get; set; }
        public int? CentroCustoId { get; set; }
        public string? CentroCustoNome { get; set; }
        public DateTime? DataDemissao { get; set; }
        public int MotivoDemissaoId { get; set; }
        public string? MotivoNome { get; set; }
        public bool TemContratoExperiencia { get; set; } = false;
        public int? StatusApontamento { get; set; }
        public int? StatusAvisoPrevio { get; set; }
        public int? StatusFichaEpi { get; set; }
        public int? StatusExameDemissional { get; set; }
        public DateTime? DataCredito { get; set; }
        public int? StatusBeneficio { get; set; }
        public int? StatusReembolso { get; set; }
    }

    public sealed class SolicitacaoFeriasResponse
    {
        public int SolicitacaoId { get; init; }
        public int FuncionarioId { get; set; }
        public string? FuncionarioNome { get; init; }
        public DateTime FuncionarioDataAdmissao { get; init; }
        public string? eSocialMatricula { get; set; }
        public int? CentroCustoId { get; set; }
        public string? CentroCustoNome { get; set; }
        public DateTime DataSaida { get; set; }
        public DateTime DataRetorno { get; set; }
        public int TipoFeriasId { get; set; }
        public String? TipoFeriasNome { get; set; }
    }

    public sealed class SolicitacaoMudancaBaseResponse
    {
        public int SolicitacaoId { get; init; }
        public int FuncionarioId { get; set; }
        public string? FuncionarioNome { get; init; }
        public DateTime FuncionarioDataAdmissao { get; init; }
        public string? eSocialMatricula { get; set; }
        public int? CentroCustoId { get; set; }
        public string? CentroCustoNome { get; set; }
        public DateTime? DataAlteracao { get; init; }
        public string? Observacao { get; init; }
        public int? ClienteDestinoId { get; init; }
        public string? ClienteDestinoNome { get; init; }
        public List<ListItem>? ItensMudanca { get; init; } = new List<ListItem>();
    }

    public sealed class SolicitacaoVagaResponse
    {
        public int SolicitacaoId { get; init; }
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
    }
      


}