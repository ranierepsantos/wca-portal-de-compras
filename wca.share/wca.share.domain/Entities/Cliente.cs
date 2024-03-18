using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using wca.share.domain.Common.Interfaces;

namespace wca.share.domain.Entities
{

    [Table("Clientes")]
    public sealed class Cliente : IEntity
    {
        [Column("id")]
        public int Id { get; set; }

        [Column("filial_id")]
        public int FilialId { get; set; }

        [Required, MaxLength(150)]
        [Column("nome", TypeName = "varchar(150)")]
        public string Nome { get; set; }

        [Required, MaxLength(20)]
        [Column("cnpj", TypeName = "varchar(20)")]
        public string CNPJ { get; set; }

        [MaxLength(20)]
        [Column("inscricao_estadual", TypeName = "varchar(20)")]
        public string? InscricaoEstadual { get; set; }

        [MaxLength(150)]
        [Column("endereco", TypeName = "varchar(150)")]
        public string? Endereco { get; set; }

        [MaxLength(9)]
        [Column("cep", TypeName = "varchar(9)")]
        public string? Cep { get; set; }

        [MaxLength(10)]
        [Column("numero", TypeName = "varchar(10)")]
        public string? Numero { get; set; }

        [MaxLength(100)]
        [Column("cidade", TypeName = "varchar(100)")]
        public string? Cidade { get; set; }

        [MaxLength(2)]
        [Column("uf", TypeName = "varchar(2)")]
        public string? UF { get; set; }

        [Column("ativo")]
        public bool Ativo { get; set; } = true;

        [Column("codigo_cliente")]
        public int? CodigoCliente { get; set; } = null;

        public IList<UsuarioClientes> UsuarioClientes { get; set; } = new List<UsuarioClientes>();
        
        public IList<CentroCusto> CentroCusto { get; set; } = new List<CentroCusto>();

    }
}
