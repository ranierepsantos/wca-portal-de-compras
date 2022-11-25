using System.ComponentModel.DataAnnotations;

namespace wca.compras.domain.Dtos
{
    public record PerfilDto (int Id, string Nome, string Descricao, bool Ativo);
        
    public record PerfilPermissoesDto(int Id, string Nome, string Descricao, bool Ativo, IList<PermissoesDto> Permissao);

    public record CreatePerfilDto(
        [Required(ErrorMessage = "O campo é obrigatório!")]
        string Nome, 
        string Descricao,
        [Required(ErrorMessage = "O campo é obrigatório!"), MinLength(1, ErrorMessage = "Não pode ser nulo ou vazio!")] IList<PermissoesDto> Permissoes
    );

    public record UpdatePerfilDto(
        [Required(ErrorMessage = "O campo é obrigatório!")] 
        int Id, 
        [Required(ErrorMessage = "O campo é obrigatório!")] 
        string Nome, 
        string Descricao, 
        bool Ativo, 
        [Required(ErrorMessage="O campo é obrigatório!"), MinLength(1, ErrorMessage = "Não pode ser nulo ou vazio!")] 
        IList<PermissoesDto> Permissao
    );
}
