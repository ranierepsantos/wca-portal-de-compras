using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace wca.compras.domain.Dtos
{
    public record RequisicaoPaginateFilters(
        int[]? Filials = null, 
        int AuthUserId = 0, 
        int ClienteId = 0, 
        int UsuarioId = 0, 
        int FornecedorId = 0, 
        int? CodigoRequisicao = null,
        DateTime? DataInicio = null, 
        DateTime? DataFim = null, 
        int[]? Status = null
     );
}
