using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace wca.reembolso.domain.Entities
{
    [Table("ContaCorrente")]
    public sealed class ContaCorrente
    {
        
        [Column("usuario_id")]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int UsuarioId { get; set; }
        public Usuario? Usuario { get; set; }

        [Column("saldo", TypeName ="money")]
        public decimal Saldo { get; set; } = decimal.Zero;
        public IList<Transacao> Transacoes { get; set; } = new List<Transacao>();
    }
}
