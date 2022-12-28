using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using wca.compras.domain.Interfaces;

namespace wca.compras.domain.Entities
{
    public class ClienteUsuario: IEntity
    {
        [Column("ClienteId")]
        public int ClienteId { get; set; }

        [Column("UsuarioId")]
        public int UsuarioId { get; set; }
    }
}
