using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace wca.compras.domain.Entities
{
    public class RequisicaoHistorico
    {
        [Column("id")]
        public int Id { get; set; }

        [Column("requisicao_id")]
        public int RequisicaoId { get; set; }

        [Required]
        [Column("data_hora", TypeName = "smalldatetime")]
        public DateTime DataHora { get; set; } = DateTime.UtcNow;

        [Required]
        [Column("evento", TypeName = "varchar(500)")]
        public string Evento { get; set; } = "";

    }
}
