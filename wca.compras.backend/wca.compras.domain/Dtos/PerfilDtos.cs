using System.ComponentModel.DataAnnotations;
using static wca.compras.domain.Dtos.PermissaoDtos;

namespace wca.compras.domain.Dtos
{
    public class PerfilDtos
    {
        public record PerfilDto (string Id, string Nome, string Descricao, bool Ativo);
        
        public record PerfilPermissoesDto(string Id, string Nome, string Descricao, bool Ativo, IList<PermissoesDto> Permissoes);

        public record CreatePerfilDto([Required] string Nome, string Descricao, [Required] IList<PermissoesDto> Permissoes);

        public record UpdatePerfilDto([Required] string Id, [Required] string Nome, string Descricao, bool Ativo, [Required] IList<PermissoesDto> Permissoes);

    }
}
