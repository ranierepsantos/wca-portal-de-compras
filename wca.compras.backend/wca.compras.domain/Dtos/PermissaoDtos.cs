using System.ComponentModel.DataAnnotations;

namespace wca.compras.domain.Dtos
{
        public record PermissaoDto(string Id, string Nome, string Regra, string Descricao);

        public record PermissoesDto(
                [Required(ErrorMessage = "O campo é obrigatório!")] string Id, 
                [Required(ErrorMessage = "O campo é obrigatório!")] string Nome
        );

        public record CreatePermissaoDto(
            [Required(ErrorMessage = "O campo é obrigatório!")] 
            string Nome, 
            [Required(ErrorMessage = "O campo é obrigatório!")] 
            string Regra, 
            string Descricao);
        
        public record UpdatePermissaoDto(
            [Required(ErrorMessage = "O campo é obrigatório!")] 
            string Id, 
            [Required(ErrorMessage = "O campo é obrigatório!")]
            string Nome, 
            [Required(ErrorMessage = "O campo é obrigatório!")] 
            string Regra, 
            string Descricao);
}
