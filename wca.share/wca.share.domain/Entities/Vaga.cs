using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace wca.share.domain.Entities
{
    [Table("Vagas")]
    public class Vaga
    {
        [Column("id")]
        public int Id { get; set; }

        [Column("cliente_id")]
        public int ClienteId { get; set; }

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

        [Column("responsavel_id")]
        public int? ResponsavelId { get; set; }

        [Column("data_solicitacao", TypeName = "smalldatetime")]
        public DateTime DataSolicitacao { get; set; } = DateTime.Now;

        [Column("status_solicitacao_id")]
        public int StatusSolicitacaoId { get; set; } = 1;

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
        public virtual List<VagaHistorico>? VagaHistorico { get; set; } = new List<VagaHistorico>();

        [JsonIgnore]
        public Cliente? Cliente { get; set; }

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

        [JsonIgnore]
        public StatusSolicitacao? StatusSolicitacao { get; set; }

        [JsonIgnore]
        public Usuario? Responsavel { get; set; }
    }
}
