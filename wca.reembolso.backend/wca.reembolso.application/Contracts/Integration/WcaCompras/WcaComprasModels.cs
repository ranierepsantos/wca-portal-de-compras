using System.Text.Json.Serialization;

namespace wca.reembolso.application.Contracts.Integration.WcaCompras
{
    public record LoginRequest(string Email, string Password);

    public record LoginResponse(
        bool Authenticated,
        string Message,
        string Created,
        string Expiration,
        string AccessToken,
        int UsuarioId,
        string UsuarioNome
    );

    public sealed class UsuarioResponse
    {
        [JsonPropertyName("value")]
        public int Id { get; init; }

        [JsonPropertyName("text")]
        public string Nome { get; init; }
    }
}
