using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace wca.compras.domain.Dtos
{
    public record CreateFilialDto (string nome);

    public record FilialDto(int Id, string Nome, bool Ativo);
}
