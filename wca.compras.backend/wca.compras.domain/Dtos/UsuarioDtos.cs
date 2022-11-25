using System.ComponentModel.DataAnnotations;

namespace wca.compras.domain.Dtos
{
    public record UsuarioDto (
        int Id,    
        string Nome,
        string Email,
        bool Ativo,
        int Clienteid,
        int Filialid,
        int Perfilid
    );

    public record CreateUsuarioDto (
        [Required(ErrorMessage ="O campo é obrigatório")]
        string Nome,
        [Required(ErrorMessage ="O campo é obrigatório")]
        [EmailAddress(ErrorMessage ="Infome um e-mail válido")]
        string Email,
        [Required(ErrorMessage ="O campo é obrigatório")]
        int ClienteId,
        [Required(ErrorMessage ="O campo é obrigatório")]
        int FilialId,
        [Required(ErrorMessage ="O campo é obrigatório")]
        int PerfilId
    );

    public record UpdateUsuarioDto(
        [Required(ErrorMessage ="O campo é obrigatório")]
        int Id,
        [Required(ErrorMessage ="O campo é obrigatório")]
        string Nome,
        [Required(ErrorMessage ="O campo é obrigatório")]
        [EmailAddress(ErrorMessage ="Infome um e-mail válido")]
        string Email,
        bool Ativo,
        [Required(ErrorMessage ="O campo é obrigatório")]
        int ClienteId,
        [Required(ErrorMessage ="O campo é obrigatório")]
        int FilialId,
        [Required(ErrorMessage ="O campo é obrigatório")]
        int PerfilId
    );
}
