using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using wca.reembolso.domain.Common.Enum;

namespace wca.reembolso.domain.Entities
{
    [Table(name: "Status_ChatBot_Mensagem")]
    public class StatusChatBotMensagem
    {
        public int Id { get; set; }

        [Column("status_solicitacao_id")]
        public int StatusSolicitacaoId { get; set; }

        [JsonIgnore]
        public StatusSolicitacao StatusSolicitacao { get; set; }

        [Column("tipo_solicitacao")]
        [Comment("Tipo de solicitação que será enviado: 0 - Ambas 1 - Reembolso, 2 - Adiantamento")]
        public int TipoSolicitacao { get; set; } = 0;

        [Column("enviar_para")]
        [Comment("Tipo de usuário que será enviado: 1- Gestor WCA, 2 - Gestor Cliente, 3 - Colaborador")]
        public int EnviarPara { get; set; }

        [Column("mensagem", TypeName ="varchar(2000)")]
        public string Mensagem { get; set; }


    }
}
