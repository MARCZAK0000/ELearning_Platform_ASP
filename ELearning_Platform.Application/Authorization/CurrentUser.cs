namespace ELearning_Platform.Infrastructure.Authorization
{
    public class CurrentUser(string userID, string Email)
    {
        public string UserID = userID;  

        public string EmailAddress = Email;

       
    }
}
