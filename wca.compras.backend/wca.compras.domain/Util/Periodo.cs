using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace wca.compras.domain.Util
{
    public class DiaSemana
    {
        public List<string> periodo { get; set; }
        public bool selected { get; set; }
    }

    public class Periodo
    {
        [JsonProperty("0")]
        public DiaSemana Domingo { get; set; }

        [JsonProperty("1")]
        public DiaSemana Segunda { get; set; }

        [JsonProperty("2")]
        public DiaSemana Terca { get; set; }

        [JsonProperty("3")]
        public DiaSemana Quarta { get; set; }

        [JsonProperty("4")]
        public DiaSemana Quinta { get; set; }

        [JsonProperty("5")]
        public DiaSemana Sexta { get; set; }

        [JsonProperty("6")]
        public DiaSemana Sabado { get; set; }
    }
}
