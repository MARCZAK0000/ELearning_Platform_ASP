namespace ELearning_Platform.Domain.Response.Account
{
    public class LoginResponse
    {
        public string Email {  get; set; }
        
        public string Token { get; set; }

        public string RefreshToken { get; set; }    
        
        public DateTime SignInDate { get; set; } = DateTime.Now;

        public DateTime TokenExpiredDate { get; set; } = DateTime.Now.AddMinutes(15);
    }
}
