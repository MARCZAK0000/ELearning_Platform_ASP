using ELearning_Platform.Domain.Models.Email;
using MimeKit;

namespace ELearning_Platform.Infrastructure.EmailSender.Class
{
    public static class EmailSenderCreateMessage
    {

        public static Task<MimeMessage> CreateMessageAsync(EmailDto email, CancellationToken token)
        {
            var message = new MimeMessage();
            message.From.Add(MailboxAddress.Parse(email.From));
            message.To.Add(MailboxAddress.Parse(email.To));
            message.Subject = email.Subject;
            message.Body = new TextPart(MimeKit.Text.TextFormat.Html) { Text = email.Body };

            return Task.FromResult(message);
        }
    }
}