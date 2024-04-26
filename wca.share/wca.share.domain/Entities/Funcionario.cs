using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace wca.share.domain.Entities
{
    public sealed class Funcionario
    {
        [Column("id")]
        public int Id { get;  set; }

        [Column("nome", TypeName = "varchar(200)")]
        public string Nome { get; set; } = string.Empty;

        [Column("cliente_id")]
        public int ClienteId {  get; set; }

        [JsonIgnore]
        public Cliente? Cliente { get; set; } = null;

        [Column("centrocusto_id")]
        public int CentroCustoId { get; set; }

        [JsonIgnore]
        public CentroCusto? CentroCusto { get; set; } = null;
                
        [Column("codigo_funcionario")]
        public int? CodigoFuncionario { get; set; }

        [Column("data_admissao")]
        public DateTime? DataAdmissao { get; set; }

        [Column("data_demissao")]
        public DateTime? DataDemissao { get; set; }

        [Column("email", TypeName ="varchar(200)")]
        public string? Email { get; set; }

        [Column("ddd_celular")]
        public int? DDDCelular { get;set; }
        
        [Column("numero_celular")]
        public int? NumeroCelular { get;set; }

        [Column("e-social-matricula", TypeName = "varchar(30)")]
        public string? eSocialMatricula { get; set; } = string.Empty;
    }
}
