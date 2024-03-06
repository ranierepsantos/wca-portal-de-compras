using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace wca.reembolso.domain.Entities
{
    [Table("UsuarioCentrodeCustos")]
    public class UsuarioCentrodeCustos
    {
        public int UsuarioId {get;set;}
        
        [JsonIgnore]
        public Usuario Usuario { get;set;}


        public int CentroCustoId { get; set; }
        public CentroCusto CentroCusto { get;set;}
        

    }
}
