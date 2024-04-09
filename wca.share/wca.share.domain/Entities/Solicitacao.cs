using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace wca.share.domain.Entities
{
    public sealed class Solicitacao
    {
        [Column("id")]
        public int Id {  get; set; }

        [Column("soliticacaotipo_id")]
        public int SolicitacaoTipoId { get; set; }

        [JsonIgnore]
        public SolicitacaoTipo? SolicitacaoTipo { get; set; }

        [Column("cliente_id")]
        public int ClienteId { get; set; }

        [JsonIgnore]
        public Cliente? Cliente { get; set; }

        [Column("funcionario_id")]
        public int FuncionarioId { get; set; }

        [JsonIgnore]
        public Funcionario? Funcionario { get; set; }

        [Column("responsavel_id")]
        public int? ResponsavelId { get; set; }

        [JsonIgnore]
        public Usuario? Responsavel { get; set; }

        [Column("centrocusto_id")]
        public int CentroCustoId { get; set; }
        
        public CentroCusto? CentroCusto { get; private set; }
        
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
        public List<SolicitacaoArquivo> Anexos { get; set; } = new();
        public List<SolicitacaoHistorico> Historico { get; set; }
    }

    public sealed class SolicitacaoDesligamento
    {
        [Column("solicitacao_id")]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int SolicitacaoId { get; set; }

        [JsonIgnore]
        public Solicitacao? Solicitacao { get; set; }

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

        [Column("assunto_id")]
        public int AssuntoId { get; set; }

        [JsonIgnore]
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
}
