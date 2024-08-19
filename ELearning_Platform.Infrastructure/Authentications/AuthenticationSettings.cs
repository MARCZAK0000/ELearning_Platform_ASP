namespace ELearning_Platform.Infrastructure.Authentications
{
    public class AuthenticationSettings
    {
        public string Issure {  get; set; }
        
        public int ExpireMinutes { get; set; }

        public string Key { get; set; }  
    }
}
