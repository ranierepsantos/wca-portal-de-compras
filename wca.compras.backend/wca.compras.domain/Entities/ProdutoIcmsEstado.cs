using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace wca.compras.domain.Entities
{
    [Table("Produto_Icms_Estado")]
    public sealed class ProdutoIcmsEstado
    {
        [Column("produto_id")]
        public int ProdutoId { get; set; }

        [Column("uf", TypeName  = "varchar(2)")]
        public string UF { get; set;}

        [Column("icms", TypeName ="money")]
        public decimal Icms { get; set;} = decimal.Zero;

    }
}
