using ELearning_Platform.Domain.Email;
using ELearning_Platform.Domain.Repository;
using ELearning_Platform.Domain.Settings;
using System.Net;

namespace ELearning_Platform.Infrastructure.EmailSender.Class
{
    public class EmailSenderHelper(EmailSettings emailSettings, ClientSettings clientSettings) : IEmailSenderHelper
    {
        private readonly EmailSettings _emailSettings = emailSettings;
        private readonly ClientSettings _clientSettings = clientSettings;
        public EmailDto GenerateConfirmEmailMessage(string email, string token)
        {

            return new()
            {
                From = _emailSettings.Email,
                To = email,
                Subject = "Confirm Email",
                Body = GenerateBody(_clientSettings.Host, email, WebUtility.UrlEncode(token))
            };
        }

        private static string GenerateBody(string baseURL, string email, string encryptedToken)
        {
            return $@"
                    <div>
                      <h2>Welcome to E-Learning Platform</h2>
                      <h2>Confirm Your Email Address</h2>
                      <p>
                        Tap the button below to confirm your email address. If you didn't create
                        an account, you can safely delete this email.
                      </p>
                      <div>
                        <a href='{baseURL}confirm/email/token?email={email}&token={encryptedToken}'>
                          <button style=""height: 50px"">Confirm Email</button>
                        </a>
                      </div>
                      <p>
                        If that doesn't work, copy and paste the following link in your browser:
                      </p>
                      <a href='${baseURL}confirm/email/token?email={email}&token={encryptedToken}'>link</a>
                    </div>";
        }
    }
}
