using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace wca.reembolso.domain.Entities
{
    [Table("Perfil_TipoDespesa")]
    public class PerfilTipoDespesa
    {
        [Column("perfil_id")]
        public int PerfilId { get; set; }

        [Column("tipodespesa_id")]
        public int TipoDespesaId { get; set; }

        public TipoDespesa? TipoDespesa { get; set; }
    }
}