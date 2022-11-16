using System.ComponentModel.DataAnnotations;

namespace wca.compras.domain.Dtos
{
    public record UsuarioDto (
        string Id,    
        string Nome,
        string Email,
        bool Ativo,
        string cliente_id,
        string filial_id,
        string perfil_id
    );

    public record CreateUsuarioDto (
        [Required(ErrorMessage ="O campo é obrigatório")]
        string Nome,
        [Required(ErrorMessage ="O campo é obrigatório")]
        [EmailAddress(ErrorMessage ="Infome um e-mail válido")]
        string Email,
        [Required(ErrorMessage ="O campo é obrigatório")]
        string cliente_id,
        [Required(ErrorMessage ="O campo é obrigatório")]
        string filial_id,
        [Required(ErrorMessage ="O campo é obrigatório")]
        string perfil_id
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
        string cliente_id,
        [Required(ErrorMessage ="O campo é obrigatório")]
        string filial_id,
        [Required(ErrorMessage ="O campo é obrigatório")]
        string perfil_id
    );
}
