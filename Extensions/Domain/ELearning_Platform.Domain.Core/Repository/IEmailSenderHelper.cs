using ELearning_Platform.Domain.Models.Email;

namespace ELearning_Platform.Domain.Core.Repository
{
    public interface IEmailSenderHelper
    {
        public EmailDto GenerateConfirmEmailMessage(string email, string token);

    }
}
