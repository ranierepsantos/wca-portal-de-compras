using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace wca.share.domain.Entities
{
    public sealed class SolicitacaoTipo
    {
        [Column("id")]
        public int Id { get; set; }
        
        [Column("tipo", TypeName ="varchar(50)")]
        public string Tipo { get; set; } = string.Empty;
    }
}
