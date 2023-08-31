using wca.reembolso.domain.Entities;

namespace wca.reembolso.application.Features.Conta.Common
{
    public record ContaCorrenteResponse(
       int UsuarioId,
       decimal Saldo,
       IList<Transacao> Transacoes
    );
}
