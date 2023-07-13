using domain.Contracts;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace domain.Entities
{
    [Table("DespesaTipos")]
    public sealed class TipoDespesa: IEntity
    {
        [Column(name:"id", TypeName = "int")]
        public int Id { get; set; }

        [Column(name: "nome", TypeName = "varchar(50)")]
        public string Nome { get; set; } = string.Empty;
        
        [Column(name: "ativo")]
        public bool Ativo { get; set; } = true;
    }
}
