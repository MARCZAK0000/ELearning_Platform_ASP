using ELearning_Platform.Domain.Email;

namespace ELearning_Platform.Domain.Repository
{
    public interface IEmailSender
    {
        Task SendEmailAsync(EmailDto email, CancellationToken token);
    }
}
