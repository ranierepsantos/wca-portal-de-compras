using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using wca.compras.domain.Interfaces;
using System.Text.Json.Serialization;

namespace wca.compras.domain.Entities
{
    public class Recorrencia : IEntity
    {
        [Column("id")]
        public int Id { get; set; }

        [Required, MaxLength(150)]
        [Column("nome", TypeName = "varchar(150)")]
        public string Nome { get; set; }

        [Column("cliente_id")]
        public int ClienteId { get; set; }

        [Column("fornecedor_id")]
        public int FornecedorId { get; set; }

        [Column("usuario_id")]
        public int UsuarioId { get; set; }

        [Column("filial_id")]
        public int FilialId { get; set; }

        [Column("tipo_recorrencia")]
        public int TipoRecorrencia { get; set; } //0 - semanal ; 1 - mensal

        [Column("dia")]
        public int Dia { get; set; } // semanal de 0 - 7; mensal - 01 a 31

        [Column("data_criacao", TypeName = "smalldatetime")]
        public DateTime DataCriacao { get; set; } = DateTime.Now;

        public bool Ativo { get; set; } = true;

        [Column("destino", TypeName = "tinyint")]
        public EnumDestinoRequisicao Destino { get; set; } = 0;

        [Column("url_origin", TypeName = "varchar(1000)")]
        public string UrlOrigin { get; set; } = string.Empty;

        [JsonIgnore]
        public Filial? Filial { get; set; }

        [JsonIgnore]
        public Cliente? Cliente { get; set; }

        [JsonIgnore]
        public Usuario? Usuario { get; set; }

        [JsonIgnore]
        public Fornecedor? Fornecedor { get; set; }
        
        public IList<RecorrenciaProduto> RecorrenciaProdutos { get; set; } = new List<RecorrenciaProduto>();

        public IList<RecorrenciaLog> RecorrenciaLogs { get; set; } = new List<RecorrenciaLog>();
    }

    public class RecorrenciaProduto : IEntity
    {
        [Column("id")]
        public int Id { get; set; }

        [Column("recorrencia_id")]
        public int RecorrenciaId { get; set; }

        [Required, MaxLength(50)]
        [Column("codigo", TypeName = "varchar(50)")]
        public string Codigo { get; set; }

        [Required, MaxLength(200)]
        [Column("nome", TypeName = "varchar(200)")]
        public string Nome { get; set; }

        [Required, MaxLength(20)]
        [Column("unidade_medida", TypeName = "varchar(20)")]
        public string UnidadeMedida { get; set; }

        [Required]
        [Column("quantidade", TypeName = "int")]
        public int Quantidade { get; set; }

        [JsonIgnore]
        public Recorrencia? Recorrencia { get; set; }

    }

    public class RecorrenciaLog : IEntity
    {
        [Column("id")]
        public int Id { get; set; }

        [Column("recorrencia_id")]
        public int RecorrenciaId { get; set; }

        [Column("data_hora", TypeName = "smalldatetime")]
        public DateTime DataHora { get; set; } = DateTime.Now;

        [Required]
        [Column("status", TypeName = "varchar(15)")]
        public string Status { get; set; } = "";


        [Required]
        [Column("log", TypeName = "varchar(5000)")]
        public string Log { get; set; } = "";

        [JsonIgnore]
        public Recorrencia? Recorrencia { get; set; }
    }
}
