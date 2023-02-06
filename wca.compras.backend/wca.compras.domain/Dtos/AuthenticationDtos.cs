using Org.BouncyCastle.Asn1.X509;
using System.ComponentModel.DataAnnotations;
using wca.compras.domain.Entities;
using wca.compras.domain.Util;

namespace wca.compras.domain.Dtos
{
    public record LoginRequest ([Required(ErrorMessage = "O campo é obrigatório"), EmailAddress(ErrorMessage = "Informe um e-mail válido!")] string Email, [Required(ErrorMessage = "O campo é obrigatório")]  string Password);

    public record LoginResponse(
        bool Authenticated,
        string Message,
        string Created,
        string Expiration,
        string AccessToken,
        int UsuarioId,
        int? FilialId,
        string UsuarioNome,
        PerfilPermissoesDto Perfil
    );
    
    public record ForgotPasswordRequest (
        [Required(ErrorMessage = "O campo é obrigatório!")]
        [EmailAddress(ErrorMessage = "Informe um e-mail válido!")]
        string Email
    );

    public class ResetPasswordRequest {
        [Required(ErrorMessage = "O campo é obrigatório!")]
        public string Token { get; set; }

        [Required(ErrorMessage = "O campo é obrigatório")]
        public string Password { get; set; }

        [Required(ErrorMessage = "O campo é obrigatório")]
        [Compare("Password", ErrorMessage ="Password e ConfirmPassword deve ser iguais")]
        public string ConfirmPassword { get; set; }
    }
}
