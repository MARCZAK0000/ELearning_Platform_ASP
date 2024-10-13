using ELearning_Platform.Domain.Email;

namespace ELearning_Platform.Infrastructure.EmailSender.Interface
{
    public interface IEmailSenderHelper
    {
        public EmailDto GenerateConfirmEmailMessage(string email, string token);

    }
}
