using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace wca.reembolso.domain.Entities
{
    public sealed class Notificacao
    {
        [Column("id")]
        public int Id { get; set; }

        [Column("usuario_id")]
        public int UsuarioId { get; set; }

        [Column("data_hora", TypeName = "smalldatetime")]
        public DateTime DataHora { get; set; } = DateTime.Now;

        [Column("nota", TypeName = "varchar(500)")]
        public string Nota { get; set; } = "";

        [Column("lido")]
        public bool Lido { get; set; } = false;
    }
}
