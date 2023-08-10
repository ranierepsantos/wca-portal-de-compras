using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace wca.reembolso.application.Features.Clientes.Common
{
    public record ClienteResponse (
        int Id,
        int FilialId, 
        string Nome,
        string CNPJ,
        string InscricaoEstadual,
        string Endereco,
        string Numero,
        string CEP,
        string Cidade,
        string UF,
        bool Ativo,
        decimal ValorLimite
    );
}
