using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using wca.reembolso.domain.Common.Enum;

namespace wca.reembolso.application.Features.TiposDespesa.Common
{
    public record TipoDespesaResponse(
        int Id,
        string Nome,
        EnumTipoDespesaTipo Tipo,
        bool Ativo,
        decimal Valor
    );
}
