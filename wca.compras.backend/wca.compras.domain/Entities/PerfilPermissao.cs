using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace wca.compras.domain.Entities
{
    [NotMapped]
    public class PerfilPermissao
    {
        [Column("perfil_id")]
        public int PerfilId { get; set; }

        [Column("permissao_id")]
        public int PermissaoId { get; set; }

        public virtual Perfil Perfil { get; set; }
        
        public virtual Permissao Permissao { get; set; }
    }
}
