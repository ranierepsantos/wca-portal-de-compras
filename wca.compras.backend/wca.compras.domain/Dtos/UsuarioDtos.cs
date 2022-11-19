using System.ComponentModel.DataAnnotations;

namespace wca.compras.domain.Dtos
{
    public record UsuarioDto (
        string Id,    
        string Nome,
        string Email,
        bool Ativo,
        string Clienteid,
        string Filialid,
        string Perfilid
    );

    public record CreateUsuarioDto (
        [Required(ErrorMessage ="O campo é obrigatório")]
        string Nome,
        [Required(ErrorMessage ="O campo é obrigatório")]
        [EmailAddress(ErrorMessage ="Infome um e-mail válido")]
        string Email,
        [Required(ErrorMessage ="O campo é obrigatório")]
        string ClienteId,
        [Required(ErrorMessage ="O campo é obrigatório")]
        string FilialId,
        [Required(ErrorMessage ="O campo é obrigatório")]
        string PerfilId
    );

    public record UpdateUsuarioDto(
        [Required(ErrorMessage ="O campo é obrigatório")]
        string Id,
        [Required(ErrorMessage ="O campo é obrigatório")]
        string Nome,
        [Required(ErrorMessage ="O campo é obrigatório")]
        [EmailAddress(ErrorMessage ="Infome um e-mail válido")]
        string Email,
        bool Ativo,
        [Required(ErrorMessage ="O campo é obrigatório")]
        string ClienteId,
        [Required(ErrorMessage ="O campo é obrigatório")]
        string FilialId,
        [Required(ErrorMessage ="O campo é obrigatório")]
        string PerfilId
    );
}
