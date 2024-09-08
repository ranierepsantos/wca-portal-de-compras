using System.ComponentModel.DataAnnotations.Schema;

namespace wca.share.domain.Entities
{
    public class EventLogGi
    {
        [Column("id", TypeName ="bigint")]
        public ulong Id { get; set; }

        [Column("data_hora", TypeName = "varchar(50)")]
        public DateTime DataHora { get; set; } = DateTime.UtcNow;

        [Column("entidade", TypeName = "varchar(50)")]
        public string Entidade { get; set; } = string.Empty;

        [Column("log", TypeName ="varchar(max)")]
        public string Log { get; set; }
    }
}
