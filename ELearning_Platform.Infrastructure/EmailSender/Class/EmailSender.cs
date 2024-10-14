using ELearning_Platform.Domain.Email;
using ELearning_Platform.Domain.Repository;
using ELearning_Platform.Domain.Settings;
using MailKit.Net.Smtp;
using System.Net;

namespace ELearning_Platform.Infrastructure.EmailSender.Class
{
    public class EmailSender(EmailSettings settings) : IEmailSender
    {
        private readonly EmailSettings _settings = settings;
        public async Task SendEmailAsync(EmailDto email, CancellationToken token)
        {
            var message = await EmailSenderCreateMessage.CreateMessageAsync(email, token);
            using var smtp = new SmtpClient();
            await smtp.ConnectAsync(host: _settings.Host, port: _settings.Port, MailKit.Security.SecureSocketOptions.StartTls, cancellationToken: token);
            await smtp.AuthenticateAsync(credentials: new NetworkCredential(userName: _settings.Email, password: _settings.Password), cancellationToken: token);
            await smtp.SendAsync(message, cancellationToken: token);
            await smtp.DisconnectAsync(true, token);
        }
    }
}
