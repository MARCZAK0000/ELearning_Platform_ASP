using ELearning_Platform.Domain.Email;

namespace ELearning_Platform.Domain.Repository
{
    public interface IEmailSenderHelper
    {
        public EmailDto GenerateConfirmEmailMessage(string email, string token);

    }
}
