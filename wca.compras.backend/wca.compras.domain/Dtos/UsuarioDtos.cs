using System.ComponentModel.DataAnnotations;
using wca.compras.domain.Util;

namespace wca.compras.domain.Dtos
{
    public record UsuarioDto (
        int Id,    
        string Nome,
        string Email,
        bool Ativo,
        int Filialid,
        int Perfilid,
        FilialDto Filial,
        IList<ListItem> Cliente,
        IList<ListItem> TipoFornecimento
    );

    public record CreateUsuarioDto (
        [Required(ErrorMessage ="O campo é obrigatório")]
        string Nome,
        [Required(ErrorMessage ="O campo é obrigatório")]
        [EmailAddress(ErrorMessage ="Infome um e-mail válido")]
        string Email,
        [Required(ErrorMessage ="O campo é obrigatório")]
        int FilialId,
        [Required(ErrorMessage ="O campo é obrigatório")]
        int PerfilId,
        IList<ListItem> Cliente,
        IList<ListItem> TipoFornecimento
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
        int FilialId,
        [Required(ErrorMessage ="O campo é obrigatório")]
        int PerfilId,
        IList<ListItem> Cliente,
        IList<ListItem> TipoFornecimento
    );
}
