using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace wca.reembolso.application.Features.Notificacoes.Common
{
    public record NotificacaoResponse(
        int Id,
        DateTime DataHora,
        string Nota
    );
}
