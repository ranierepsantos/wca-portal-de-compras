using MimeKit;

namespace wca.share.application.Contracts.Integration.Email
{
    public interface IEmailService
    {
        void Send(MimeMessage mailMessage);

        void SendNotificacao(string[] emails, string assunto, string notificacao);
    }
}
