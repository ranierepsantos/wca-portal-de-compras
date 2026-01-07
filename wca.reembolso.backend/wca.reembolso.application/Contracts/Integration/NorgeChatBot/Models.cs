using System.Text.Json.Serialization;

namespace wca.reembolso.application.Contracts.Integration.NorgeChatBot
{
    public sealed class ChatBotMessage
    {
        public string Phone { get; init; }
        public string Message { get; init; }

        public ChatBotMessage(string number, string body)
        {
            Phone = number;
            Message = body;
        }
    }

    //public sealed class Response
    //{
    //    [JsonPropertyName("mensagem")]
    //    public string? Mensagem { get; set; }

    //    [JsonPropertyName("error")]
    //    public string? Error { get; set; }
    //}

    public class Rootobject
    {
        public int statusCode { get; set; }
        public Data data { get; set; }
    }

    public class Data
    {

        public bool success { get; set; }
        public Data1 data { get; set; }
        public string message { get; set; }
        public string curl { get; set; }
        public Meta_Error meta_error { get; set; }
    }

    public class Data1
    {
        public int status { get; set; }
        public string error { get; set; }
        public Response response { get; set; }
    }

    public class Response
    {
        public string[] message { get; set; }
    }

    public class Meta_Error
    {
        public bool success { get; set; }
        public Data2 data { get; set; }
        public string message { get; set; }
    }

    public class Data2
    {
        public Error error { get; set; }
    }

    public class Error
    {
        public string message { get; set; }
        public string type { get; set; }
        public int code { get; set; }
        public int error_subcode { get; set; }
        public string fbtrace_id { get; set; }
    }


}
