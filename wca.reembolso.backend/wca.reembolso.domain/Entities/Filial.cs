using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace wca.reembolso.domain.Entities
{
    public sealed class Filial
    {
        [Column("id")]
        public int Id { get; set; }

        [Required, MaxLength(150)]
        [Column("nome", TypeName = "varchar(150)")]
        public string Nome { get; set; } = string.Empty;

        [Column("ativo")]
        public bool Ativo { get; set; } = true;

        [JsonIgnore]
        public IList<FilialUsuario> FilialUsuario { get; set; } = new List<FilialUsuario>();
    }
}
