using MimeKit;
using System;
using System.Collections.Generic;
using System.Linq;
using MailKit.Net.Smtp;
using System.Text;
using System.Threading.Tasks;
using wca.compras.domain.Email;
using wca.compras.domain.Interfaces.Services;
using MimeKit.Utils;
using wca.compras.domain.Entities;

namespace wca.compras.services
{
  public class EmailService : IEmailService
  {
    private readonly EmailConfiguration _emailConfig;
    public EmailService(EmailConfiguration emailConfig)
    {
      _emailConfig = emailConfig;
    }
    public void SendEmail(Message message)
    {
      var emailMessage = CreateEmailMessage(message);
      Send(emailMessage);
    }

    private MimeMessage CreateEmailMessage(Message message)
    {
      var emailMessage = new MimeMessage();
      emailMessage.From.Add(new MailboxAddress("WCA Gestão Compras", _emailConfig.From));
      emailMessage.To.AddRange(message.To);
      emailMessage.Subject = message.Subject;
      emailMessage.Body = new TextPart(MimeKit.Text.TextFormat.Html) { Text = message.Content };

      return emailMessage;
    }

    public void SendRequisicaoFornecedorEmail()
    {
      var currentDirectory = Directory.GetCurrentDirectory();
      var parentDirectory = Directory.GetParent(currentDirectory).FullName;
      var filesDirectory = Path.Combine(parentDirectory, "Files");

      BodyBuilder builder = new BodyBuilder();
      var imagePath = Path.Combine(filesDirectory, "logoWCA.png");

      var image = builder.LinkedResources.Add(imagePath);
      image.ContentId = MimeUtils.GenerateMessageId();

      
      var bodyHtml = "<p style='color:grey; text-align: center;font-size:36px'>Você recebeu um novo pedido da WCA.<br/>";
        bodyHtml += "Clique no botão abaixo para acessar o <br/>";
        bodyHtml += "pedido<p><br/>";
        bodyHtml += "<p style='text-align: center;'><a href='https://www.google.com.br' style='font: bold 18px Arial; text-decoration: none;background-color: #000066; color: white; padding: 1em 1.5em; border-radius: 5%;'>Acessar pedido</a></p>";
      

      builder.HtmlBody = string.Format(@"<p style='text-align:center;'><img src='cid:{0}' width='250px'; height='172.25px';></p>{1}", image.ContentId, bodyHtml);
      

      var message = new Message(new string[] { "ivanildo.bacelar@outlook.com.br" }, "Novo pedido WCA", "");
      var emailMessage = new MimeMessage();
      emailMessage.From.Add(new MailboxAddress("WCA Gestão Compras", _emailConfig.From));
      emailMessage.To.AddRange(message.To);
      emailMessage.Subject = message.Subject;
      emailMessage.Body = builder.ToMessageBody();

      Send(emailMessage);

    }

    private void Send(MimeMessage mailMessage)
    {
      using (var client = new SmtpClient())
      {
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
          Console.WriteLine($"Erro ao enviar e-mail: {ex.Message}");
          throw new Exception(ex.Message, ex.InnerException);
        }
        finally
        {
          client.Disconnect(true);
          client.Dispose();
        }
      }
    }

  }
}
