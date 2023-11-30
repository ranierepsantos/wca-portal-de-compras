using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace wca.reembolso.application.Features.Filiais.Common
{
    public sealed record FilialResponse (
        int Id,
        string Nome,
        bool Ativo
    );
}
