using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace wca.compras.domain.Dtos
{
    public record TipoFornecimentoDto (int Id, string Nome, bool Ativo);

    public record CreateTipoFornecimentoDto(string Nome);

}
