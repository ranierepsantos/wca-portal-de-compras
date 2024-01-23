using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using wca.reembolso.application.Features.Solicitacoes.Common;
using wca.reembolso.domain.Entities;

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
        decimal ValorLimite,
        string? CodigoCliente,
        IList<CentroCusto> CentroCusto
    );

    public record ClienteResponseWithValorUsado(
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
        decimal ValorLimite,
        decimal ValorUtilizadoMes,
        string? CodigoCliente
    );
}
