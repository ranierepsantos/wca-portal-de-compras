using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace wca.share.domain.Entities
{
    public sealed class Solicitacao
    {
        [Column("id")]
        public int Id {  get; set; }

        [Column("solicitacaotipo_id")]
        public int SolicitacaoTipoId { get; set; }

        [JsonIgnore]
        public SolicitacaoTipo? SolicitacaoTipo { get; set; }

        [Column("cliente_id")]
        public int ClienteId { get; set; }

        [JsonIgnore]
        public Cliente? Cliente { get; set; }

        [Column("responsavel_id")]
        public int? ResponsavelId { get; set; }

        [JsonIgnore]
        public Usuario? Responsavel { get; set; }

        [Column("data_solicitacao", TypeName = "smalldatetime")]
        public DateTime DataSolicitacao { get; set; } = DateTime.Now;
        
        [Column("descricao", TypeName = "nvarchar(max)")]
        public string? Descricao { get; set; } = string.Empty;

        [Column("status_id")]
        public int StatusSolicitacaoId { get; set; } = 1;

        [JsonIgnore]
        public StatusSolicitacao? StatusSolicitacao { get; set; }
        public SolicitacaoComunicado? Comunicado { get; set; }
        public SolicitacaoDesligamento? Desligamento { get; set; }
        public SolicitacaoMudancaBase? MudancaBase { get; set; }
        public SolicitacaoFerias? Ferias { get; set; }
        public List<SolicitacaoArquivo> Anexos { get; set; } = new();
        public List<SolicitacaoHistorico> Historico { get; set; }
        public SolicitacaoVaga? Vaga { get; set; }
    }

    public sealed class SolicitacaoDesligamento
    {
        [Column("solicitacao_id")]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int SolicitacaoId { get; set; }

        [JsonIgnore]
        public Solicitacao? Solicitacao { get; set; }

        [Column("funcionario_id")]
        public int FuncionarioId { get; set; }

        [JsonIgnore]
        public Funcionario? Funcionario { get; set; }

        [Column("centrocusto_id")]
        public int CentroCustoId { get; set; }
        public CentroCusto? CentroCusto { get; set; }


        [Column("data_demissao")]
        public DateTime? DataDemissao { get; set;}

        [Column("motivodemissao_id")]
        public int MotivoDemissaoId { get; set; }
        
        [JsonIgnore]
        public MotivoDemissao? Motivo { get; set;}

        [Column("tem_contrato_experiencia")]
        public bool TemContratoExperiencia { get; set; } = false;
        
        [Column("status_apontamento")]
        public int? StatusApontamento { get; set; }
        
        [Column("status_aviso_previo")] 
        public int? StatusAvisoPrevio { get; set; }
        
        [Column("status_ficha_epi")]
        public int? StatusFichaEpi { get; set; }

        [Column("status_exame_demissional")]
        public int? StatusExameDemissional { get; set; }

        [Column("data_credito")]
        public DateTime? DataCredito { get; set; }

        [Column("status_beneficio")]
        public int? StatusBeneficio { get; set; }

        [Column("status_reembolso")]
        public int? StatusReembolso { get; set; }
    }

    public sealed class SolicitacaoMudancaBase
    {

        [Column("solicitacao_id")]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int SolicitacaoId { get; set; }
        
        [JsonIgnore]
        public Solicitacao? Solicitacao { get; set; }

        [Column("funcionario_id")]
        public int FuncionarioId { get; set; }

        [JsonIgnore]
        public Funcionario? Funcionario { get; set; }

        [Column("centrocusto_id")]
        public int CentroCustoId { get; set; }

        public CentroCusto? CentroCusto { get; set; }

        [Column("data_alteracao")]
        public DateTime? DataAlteracao { get; set; }

        [Column("observacao")]
        public string? Observacao { get; set; } = string.Empty;

        [Column("cliente_destino_id")]
        public int? ClienteDestinoId { get; set; }
        
        [JsonIgnore]
        public Cliente? ClienteDestino { get; set; }

        public List<ItemMudanca>? ItensMudanca { get; set; } = new List<ItemMudanca>();
    }

    public sealed class SolicitacaoComunicado
    {

        [Column("solicitacao_id")]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int SolicitacaoId { get; set; }

        [JsonIgnore]
        public Solicitacao? Solicitacao { get; set; }

        [Column("funcionario_id")]
        public int FuncionarioId { get; set; }

        [JsonIgnore]
        public Funcionario? Funcionario { get; set; }

        [Column("centrocusto_id")]
        public int CentroCustoId { get; set; }

        public CentroCusto? CentroCusto { get; set; }

        [Column("assunto_id")]
        public int AssuntoId { get; set; }

        public Assunto? Assunto { get; set; }

        [Column("data_alteracao")]
        public DateTime? DataAlteracao { get; set; }

        [Column("observacao")]
        public string? Observacao { get; set; } = string.Empty;
    }

    [Table("SolicitacaoArquivos")]
    public class SolicitacaoArquivo
    {
        [Column("id")]
        public int Id { get; set; }

        [Column("solicitacao_id")]
        public int SolicitacaoId { get; set; }

        [JsonIgnore]
        public Solicitacao? Solicitacao { get; set; }

        [Column("tipo", TypeName = "varchar(50)")]
        public string? Tipo { get; set; } = string.Empty;

        [Column("descricao", TypeName ="varchar(300)")]
        public string Descricao { get; set; } = string.Empty;

        [Column("caminho_arquivo", TypeName = "varchar(500)")]
        public string CaminhoArquivo { get; set; } = string.Empty; 

    }

    [Table("TiposFerias")]
    public sealed class TipoFerias
    {
        [Column("id")]
        public int Id { get; set; }

        [Column("descricao", TypeName = "varchar(100)")]
        public string Descricao { get; set; } = string.Empty;
        
        [Column("quantidade_dias")]
        public int QuantidadeDias { get; set; }
    }

    public sealed class SolicitacaoFerias
    {
        [Column("solicitacao_id")]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int SolicitacaoId { get; set; }

        [JsonIgnore]
        public Solicitacao? Solicitacao { get; set; }

        [Column("funcionario_id")]
        public int FuncionarioId { get; set; }

        [JsonIgnore]
        public Funcionario? Funcionario { get; set; }

        [Column("centrocusto_id")]
        public int CentroCustoId { get; set; }

        public CentroCusto? CentroCusto { get; set; }


        [Column("data_saida")]
        public DateTime DataSaida { get; set; }

        [Column("data_retorno")]
        public DateTime DataRetorno { get; set; }

        [Column("tipoferias_id")]
        public int TipoFeriasId { get; set; }

        public TipoFerias? TipoFerias { get; set; }
    }

    [Table("SolicitacaoVagas")]
    public class SolicitacaoVaga
    {
        [Column("solicitacao_id")]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int SolicitacaoId { get; set; }

        [JsonIgnore]
        public Solicitacao? Solicitacao { get; set; }

        [Column("tipocontrato_id")]
        public int TipoContratoId { get; set; }

        [Column("tipofaturamento_id")]
        public int TipoFaturamentoId { get; set; }

        [Column("gestor_id")]
        public int GestorId { get; set; }

        [Column("funcao_id")]
        public int FuncaoId { get; set; }

        [Column("escala_id")]
        public int EscalaId { get; set; }

        [Column("horario_id")]
        public int HorarioId { get; set; }

        [Column("quantidade_vagas")]
        public int QuantidadeVagas { get; set; } = 1;

        [Column("sexo_id")]
        public int SexoId { get; set; } = 0;

        [Column("idade_minima")]
        public int IdadeMinima { get; set; } = 0;

        [Column("idade_maxima")]
        public int IdadeMaxima { get; set; } = 0;

        [Column("caracteristica", TypeName = "varchar(max)")]
        public string? Caracteristica { get; set; } = string.Empty;

        [Column("indicacao", TypeName = "varchar(max)")]
        public string? Indicacao { get; set; } = string.Empty;

        [Column("endereco_cliente", TypeName = "varchar(1000)")]
        public string? EnderecoCliente { get; set; } = string.Empty;

        [Column("anotacoes", TypeName = "varchar(max)")]
        public string? Anotacoes { get; set; } = string.Empty;

        [Column("motivocontratacao_id")]
        public int MotivoContratacaoId { get; set; } = 0;

        [Column("justificativa_contratacao", TypeName = "varchar(1000)")]
        public string? JustificativaContratacao { get; set; } = string.Empty;

        [Column("permite_fumante")]
        public bool PermiteFumante { get; set; } = false;

        [Column("escolaridade_id")]
        public int EscolaridadeId { get; set; } = 0;

        [Column("local_residencia", TypeName = "varchar(1000)")]
        public string? LocalResidencia { get; set; } = string.Empty;

        [Column("experiencia_profissional", TypeName = "varchar(1000)")]
        public string? ExperienciaProfissinal { get; set; } = string.Empty;

        [Column("descricao_atividade", TypeName = "varchar(max)")]
        public string? DescricaoAtividades { get; set; } = string.Empty;

        [Column("tem_cnh")]
        public bool TemCNH { get; set; } = false;

        [Column("cnh_categoria", TypeName = "varchar(1)")]
        public string? CategoriaCNH { get; set; }

        [Column("tem_vale_transporte")]
        public bool TemValeTransporte { get; set; } = false;

        [Column("valor_vale_transporte", TypeName = "money")]
        public decimal? ValorValeTransporte { get; set; }

        [Column("dias_transporte")]
        public int? DiasValeTransporte { get; set; }

        [Column("refeicao", TypeName = "varchar(200)")]
        public string? Refeicao { get; set; }

        [Column("outros_beneficios", TypeName = "varchar(500)")]
        public string? OutrosBeneficios { get; set; }

        [Column("salario_base", TypeName = "money")]
        public decimal? SalarioBase { get; set; }

        [Column("tem_insalubridade")]
        public bool TemInsalubridade { get; set; } = false;

        [Column("percentual_insalubridade", TypeName = "decimal(5,2)")]
        public decimal PercentualInsalubridade { get; set; } = decimal.Zero;

        [Column("tem_periculosidade")]
        public bool TemPericulosidade { get; set; } = false;

        [Column("percentual_periculosidade", TypeName = "decimal(5,2)")]
        public decimal PercentualPericulosidade { get; set; } = decimal.Zero;

        [Column("data_inicio_prevista", TypeName = "smalldatetime")]
        public DateTime DataInicioPrevista { get; set; } = DateTime.Now;

        [Column("tem_copia_admissao_cliente")]
        public bool TemCopiaAdmissaoCliente { get; set; } = false;

        [Column("tem_integracao_cliente")]
        public bool TemIntegracaoCliente { get; set; } = false;

        [Column("horario_integracao", TypeName = "varchar(200)")]
        public string? HorarioIntegracao { get; set; }

        [Column("integracao_dias_semana", TypeName = "varchar(30)")]
        public string? IntegracaoDiasSemana { get; set; }

        public virtual List<DocumentoComplementar>? DocumentoComplementares { get; set; } = new List<DocumentoComplementar>();
        
        [JsonIgnore]
        public TipoContrato? TipoContrato { get; set; }

        [JsonIgnore]
        public Gestor? Gestor { get; set; }

        [JsonIgnore]
        public Funcao? Funcao { get; set; }

        [JsonIgnore]
        public Escala? Escala { get; set; }

        [JsonIgnore]
        public Horario? Horario { get; set; }

        [JsonIgnore]
        public MotivoContratacao? MotivoContratacao { get; set; }

        [JsonIgnore]
        public Escolaridade? Escolaridade { get; set; }

        [JsonIgnore]
        public Sexo? Sexo { get; set; }

        [JsonIgnore]
        public TipoFaturamento? TipoFaturamento { get; set; }
        
    }
}
