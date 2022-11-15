using System.ComponentModel.DataAnnotations;

namespace wca.compras.domain.Dtos
{
    public class PermissaoDtos
    {
        public record PermissaoDto(string Id, string Nome, string Regra, string Descricao);

        public record PermissoesDto([Required] string Id, [Required] string Nome);

        public record CreatePermissaoDto([Required] string Nome, [Required] string Regra, string Descricao);
        
        public record UpdatePermissaoDto([Required] string Nome, [Required] string Regra, string Descricao);
    }
}
