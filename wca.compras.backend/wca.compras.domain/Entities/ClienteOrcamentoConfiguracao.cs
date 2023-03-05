using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using wca.compras.domain.Interfaces;

namespace wca.compras.domain.Entities
{
    public class ClienteOrcamentoConfiguracao : IEntity
    {
        [Column("id")]
        public int Id { get; set; } 

        [Required, Column("cliente_id")]
        public int ClienteId { get; set; }

        [Required, Column("tipofornecimento_id")]
        public int TipoFornecimentoId { get; set; }

        [Column("valor_pedido", TypeName = "money")]
        public decimal ValorPedido { get; set; }

        [Column("quantidade_mes", TypeName = "int")]
        public decimal QuantidadeMes { get; set; }

        [Column("tolerancia", TypeName = "int")]
        public decimal Tolerancia { get; set; }

        [Column("aprovador_por", TypeName = "tinyint")]
        public EnumAprovadoPor AprovadoPor { get; set; }

        [Column("ativo")]
        public bool Ativo { get; set; } = false;

        [JsonIgnore]
        public Cliente Cliente { get; set; }

        [JsonIgnore]
        public TipoFornecimento TipoFornecimento { get; set; }
    }

    public enum EnumAprovadoPor
    {
        WCA,
        Cliente
    }
}
