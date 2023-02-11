using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using wca.compras.domain.Interfaces;

namespace wca.compras.domain.Entities
{
    public class FornecedorContato : IEntity
    {
        [Column("id")]
        public int Id { get; set; }

        [Required, MaxLength(150)]
        [Column("nome", TypeName = "varchar(150)")]
        public string Nome { get; set; }

        [MaxLength(150)]
        [Column("email", TypeName = "varchar(150)")]
        public string? Email { get; set; }

        [MaxLength(20)]
        [Column("telefone", TypeName = "varchar(20)")]
        public string? Telefone { get; set; }

        [MaxLength(20)]
        [Column("celular", TypeName = "varchar(20)")]
        public string? Celular { get; set; }

        [Column("aprova_pedido")]
        public bool AprovaPedido { get; set; }

        [Column("fornecedor_id")]
        public int? FornecedorId { get; set; }

        [JsonIgnore]
        public Fornecedor Fornecedor { get; set; }
    }

   
}
