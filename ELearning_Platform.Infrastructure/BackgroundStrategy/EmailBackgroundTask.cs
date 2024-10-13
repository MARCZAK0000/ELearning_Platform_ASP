using ELearning_Platform.Domain.BackgroundTask;
using ELearning_Platform.Domain.Email;
using ELearning_Platform.Infrastructure.EmailSender.Interface;

namespace ELearning_Platform.Infrastructure.BackgroundStrategy
{
    public class EmailBackgroundTask(IEmailSender emailSender) : IBackgroundTask
    {
        private readonly IEmailSender _emailSender = emailSender;

        public async Task ExecuteAsync(object parametrs, CancellationToken token)
        {
            var email = parametrs as EmailDto;
            if (email == null)
            {
                ArgumentException.ThrowIfNullOrEmpty(nameof(email));
            }
            await _emailSender.SendEmailAsync(email!, token);
        }
    }
}
