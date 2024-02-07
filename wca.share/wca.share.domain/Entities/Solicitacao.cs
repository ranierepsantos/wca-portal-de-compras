using System.ComponentModel.DataAnnotations.Schema;

namespace wca.share.domain.Entities
{
    public sealed class Solicitacao
    {
        [Column("id")]
        public int Id {  get; set; }

        [Column("cliente_id")]
        public int ClienteId { get; set; }

        [NotMapped]
        public Cliente Cliente { get; set; }

        [Column("funcionario_id")]
        public int FuncionarioId { get; set; }
        public Funcionario Funcionario { get; set; }

        [Column("responsavel_id")]
        public int ResponsavelId { get; set; }

        [Column("gestor_id")]
        public int GestorId { get; set; }

        [Column("data_solicitacao", TypeName = "smalldatetime")]
        public DateTime DataSolicitacao { get; set; } = DateTime.Now;
        
        [Column("descricao", TypeName = "nvarchar(max)")]
        public string Descricao { get; set; } = string.Empty;

    }
}
