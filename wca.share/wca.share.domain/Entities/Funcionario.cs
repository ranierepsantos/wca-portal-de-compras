using System.ComponentModel.DataAnnotations.Schema;

namespace wca.share.domain.Entities
{
    public sealed class Funcionario
    {
        public Funcionario(int id, string codigoGI, string nome)
        {
            Id = id;
            CodigoGI = codigoGI;
            Nome = nome;
        }

        [Column("id")]
        public int Id { get; private set; }
        [Column("codigo_gi", TypeName ="varchar(100)")]
        public string CodigoGI { get; private set; } = string.Empty;
        
        [Column("nome", TypeName = "varchar(200)")]
        public string Nome { get; private set; } = string.Empty;

        


    }
}
