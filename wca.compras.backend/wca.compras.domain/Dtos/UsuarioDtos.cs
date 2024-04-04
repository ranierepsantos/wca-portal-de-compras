using System.ComponentModel.DataAnnotations;
using wca.compras.domain.Entities;
using wca.compras.domain.Util;

namespace wca.compras.domain.Dtos
{
    public record UsuarioDto (
        int Id,    
        string Nome,
        string Email,
        bool Ativo,
        IList<UsuarioSistemaPerfil> UsuarioSistemaPerfil,
        string? Celular,
        IList<ListItem>? Filial,
        IList<ListItem>? Cliente,
        IList<ListItem>? TipoFornecimento,
        UsuarioReembolsoComplemento? UsuarioReembolsoComplemento,
        IList<UsuarioConfiguracoes>? UsuarioConfiguracoes
    );

    public record CreateUsuarioDto (
        [Required(ErrorMessage ="O campo é obrigatório")]
        string Nome,
        [Required(ErrorMessage ="O campo é obrigatório")]
        [EmailAddress(ErrorMessage ="Infome um e-mail válido")]
        string Email,
        [Required(ErrorMessage ="O campo é obrigatório")]
        IList<UsuarioSistemaPerfil> UsuarioSistemaPerfil,
        string? Celular,
        IList<ListItem>? Filial,
        IList<ListItem>? Cliente,
        IList<ListItem>? TipoFornecimento,
        UsuarioReembolsoComplemento? UsuarioReembolsoComplemento,
        IList<UsuarioConfiguracoes>? UsuarioConfiguracoes
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
        IList<UsuarioSistemaPerfil> UsuarioSistemaPerfil,
        [MinLength(1, ErrorMessage ="Informe pelo menos 1 filial")]
        IList<ListItem>? Filial,
        string? Celular,
        IList<ListItem>? Cliente,
        IList<ListItem>? TipoFornecimento,
        UsuarioReembolsoComplemento? UsuarioReembolsoComplemento,
        IList<UsuarioConfiguracoes>? UsuarioConfiguracoes
    );
}
