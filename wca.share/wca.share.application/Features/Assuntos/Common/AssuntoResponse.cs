using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace wca.share.application.Features.Assuntos.Common
{
    public record AssuntoResponse
    (
        int Id,
        string Nome,
        bool Ativo
    );
}
