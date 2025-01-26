namespace ELearning_Platform.Infrastructure.Identity.Authentications
{
    public class AuthenticationSettings
    {
        public string Issure { get; set; }

        public int ExpireMinutes { get; set; }

        public string Key { get; set; }

        public string Audience { get; set; }
    }
}
