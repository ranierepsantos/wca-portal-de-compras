using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using wca.compras.domain.Entities;

namespace wca.compras.domain.Dtos
{
    public record SistemaDto(
        int Id, 
        string Nome, 
        string Descricao,
        string Icon
    );
}
