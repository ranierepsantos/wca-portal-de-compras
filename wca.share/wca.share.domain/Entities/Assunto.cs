using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace wca.share.domain.Entities
{
    public sealed class Assunto
    {
        [Column("id")]
        public int Id { get; set; }

        [Column("nome", TypeName = "varchar(200)")]
        public string Nome { get; set; } = string.Empty;
    }
}
