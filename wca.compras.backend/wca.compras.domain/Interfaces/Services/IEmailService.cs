using wca.compras.domain.Email;

namespace wca.compras.domain.Interfaces.Services
{
    public interface IEmailService
    {
        void SendEmail(Message message);

        void SendRequisicaoParaAprovacao(string[] sentTo, string bodyHtml);
    }
}
