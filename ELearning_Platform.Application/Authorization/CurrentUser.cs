namespace ELearning_Platform.Application.Authorization
{
    public class CurrentUser(string userID, string Email)
    {
        public string UserID = userID;  

        public string EmailAddress = Email;

       
    }
}
