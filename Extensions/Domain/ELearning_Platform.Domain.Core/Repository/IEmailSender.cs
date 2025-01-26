using ELearning_Platform.Domain.Models.Email;

namespace ELearning_Platform.Domain.Core.Repository
{
    public interface IEmailSender
    {
        Task SendEmailAsync(EmailDto email, CancellationToken token);
    }
}
