using ELearning_Platform.Domain.Email;

namespace ELearning_Platform.Infrastructure.EmailSender.Interface
{
    public interface IEmailSender
    {
        Task SendEmailAsync(EmailDto email, CancellationToken token);
    }
}
