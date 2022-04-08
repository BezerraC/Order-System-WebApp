using System;
using MailKit.Net.Smtp;
using Microsoft.Extensions.Options;
using MimeKit;
using ArkdBarV1.Entities;
using System.Threading.Tasks;

namespace ArkdBarV1.Services
{
    public interface IEmailSender
    {
        Task SendEmailAsync(string email, string assunto, string mensagemTexto, string mensagemHtml);
    }

    public class EmailSender : IEmailSender
    {
        private readonly EmailConfiguration _emailConfiguration;

        public EmailSender(IOptions<EmailConfiguration> emailConfiguration)
        {
            _emailConfiguration = emailConfiguration.Value;
        }

        public async Task SendEmailAsync(string email, string assunto, string mensagemTexto, string mensagemHtml)
        {
            var mensagem = new MimeMessage();
            mensagem.From.Add(new MailboxAddress(_emailConfiguration.NomeRemetente, _emailConfiguration.EmailRemetente));
            mensagem.To.Add(MailboxAddress.Parse(email));

            mensagem.Subject = _emailConfiguration.NomeRemetente + " :: " + assunto;
            var builder = new BodyBuilder { TextBody = mensagemTexto, HtmlBody = mensagemHtml };
            mensagem.Body = builder.ToMessageBody();

            try
            {
                var smtpClient = new SmtpClient();
                smtpClient.ServerCertificateValidationCallback = (s, c, h, e) => true;
                //await client.ConnectAsync(EmailConfiguration.MailServer, _emailSettings.MailPort, _emailSettings.UseSsl).ConfigureAwait(false);
                await smtpClient.ConnectAsync(_emailConfiguration.EnderecoServidorEmail).ConfigureAwait(false);
                //se o servidor requer autenticação para enviar
                await smtpClient.AuthenticateAsync(_emailConfiguration.EmailRemetente, _emailConfiguration.Senha).ConfigureAwait(false);
                await smtpClient.SendAsync(mensagem).ConfigureAwait(false);
                await smtpClient.DisconnectAsync(true).ConfigureAwait(false);
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException(ex.Message);
            }
        }
    }
}
