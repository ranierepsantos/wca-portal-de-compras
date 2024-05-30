using System.Text.Json.Serialization;

namespace wca.reembolso.application.Contracts.Integration.NorgeChatBot
{
    public sealed class Message
    {
        public string Number { get; init; }
        public string Body { get; init; }

        public Message(string number, string body)
        {
            Number = number;
            Body = body;
        }
    }

    public sealed class Response
    {
        [JsonPropertyName("mensagem")]
        public string? Mensagem { get; set; }

        [JsonPropertyName("error")]
        public string? Error { get; set; }
    }

}
