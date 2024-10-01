using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;
using wca.reembolso.domain.Common.Enum;

namespace wca.reembolso.domain.Entities
{
    [Table("Solicitacoes")]
    public sealed class Solicitacao
    {
        [Column("id")]
        public int Id { get; set; }

        [Column("cliente_id")]
        public int ClienteId { get; set; }
        public Cliente Cliente { get; set; }

        [Column("data_solicitacao", TypeName ="smalldatetime")]
        public DateTime DataSolicitacao { get; set;} = DateTime.Today;

        [Column("colaborador_id")]
        public int ColaboradorId { get; set; }

        public Usuario? Colaborador { get; private set; }

        public int CentroCustoId { get; set; }

        public CentroCusto? CentroCusto {get; private set; }

        [Column("colaborador_cargo", TypeName = "varchar(100)")]
        public string ColaboradorCargo { get; set; }
        
        [Column("projeto", TypeName = "varchar(100)")]
        public string Projeto { get; set; }

        [Column("objetivo", TypeName = "varchar(100)")]
        public string Objetivo { get; set;}

        [Column("periodo_inicial", TypeName = "smalldatetime")]
        public DateTime? PeriodoInicial { get; set; } = null;
        
        [Column("periodo_final", TypeName = "smalldatetime")]
        public DateTime? PeriodoFinal { get;set; } = null;

        [Column("valor_adiantamento", TypeName ="money")]
        public decimal ValorAdiantamento { get; set; } = decimal.Zero;
        
        [Column("valor_despesa", TypeName = "money")]
        public decimal ValorDespesa { get;set; } = decimal.Zero;

        [Column("tipo_solicitacao")]
        public int TipoSolicitacao { get; set; } = 1;

        [Column("StatusSolicitacaoId")]
        public int Status { get; set; } = 1;

        [Column("StatusAnteriorId")]
        public int StatusAnterior { get; set; } = 0;

        [Column("data_status", TypeName = "smalldatetime")]
        public DateTime DataStatus { get; set; } = DateTime.Now;

        [Column("data_prevista_entrega", TypeName = "smalldatetime")]
        public DateTime? DataPrevistaEntrega { get; set; }

        [Column("descricao", TypeName ="varchar(1000)")]
        public string? Descricao { get; set; }

        [Column("data_entrega", TypeName = "smalldatetime")]
        public DateTime? DataEntrega { get; set; }

        public IList<Despesa> Despesa { get; set;} = new List<Despesa>();
        public IList<SolicitacaoHistorico> SolicitacaoHistorico { get; set; } = new List<SolicitacaoHistorico>();
    }
}
