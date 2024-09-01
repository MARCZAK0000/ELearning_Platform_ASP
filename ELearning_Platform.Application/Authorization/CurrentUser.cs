namespace ELearning_Platform.Application.Authorization
{
    public class CurrentUser(string userID, string Email, List<string> Roles)
    {
        public string UserID = userID;  

        public string EmailAddress = Email;

        public List<string> Roles = Roles;
    }
}
