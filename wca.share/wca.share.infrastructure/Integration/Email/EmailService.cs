using MailKit.Net.Smtp;
using MimeKit;
using wca.share.application.Contracts.Integration.Email;

namespace wca.share.infrastructure.Integration.Email
{
    public sealed class EmailService : IEmailService
    {
        private readonly EmailConfiguration _emailConfig;

        public EmailService(EmailConfiguration emailConfig)
        {
            _emailConfig = emailConfig;
        }

        public void Send(MimeMessage mailMessage)
        {
            using SmtpClient client = new();
            try
            {
                client.ServerCertificateValidationCallback = (s, c, h, e) => true;

                client.Connect(_emailConfig.SmtpServer, _emailConfig.Port, MailKit.Security.SecureSocketOptions.Auto);
                client.AuthenticationMechanisms.Remove("XOAUTH2");
                client.Authenticate(_emailConfig.UserName, _emailConfig.Password);

                client.Send(mailMessage);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex.InnerException);
            }
            finally
            {
                client.Disconnect(true);
                client.Dispose();
            }
        }

        public void SendNotificacao(string[] emails, string assunto, string notificacao)
        {

            var message = new Message(emails, "[WCA Share][NOTIFICACAO] - " + assunto, notificacao);
            var emailMessage = new MimeMessage();
            emailMessage.From.Add(new MailboxAddress("WCA Share", _emailConfig.From));
            emailMessage.To.AddRange(message.To);
            emailMessage.Subject = message.Subject;
            emailMessage.Body = new TextPart(MimeKit.Text.TextFormat.Html) { Text = message.Content };

            Send(emailMessage);
        }
    }
}
