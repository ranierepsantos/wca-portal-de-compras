using System.Text.Json.Serialization;

namespace wca.reembolso.application.Contracts.NorgeChatBot
{
    public sealed class Message
    {
        public int Number { get; set; }
        public string Body { get; set; }
    }

    public sealed class Response
    {
        [JsonPropertyName("mensagem")]
        public string Mensagem { get; set; }

        [JsonPropertyName("error")]
        public string Error { get; set; }
    }

}
