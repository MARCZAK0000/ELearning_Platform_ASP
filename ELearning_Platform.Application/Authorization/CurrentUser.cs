namespace ELearning_Platform.Infrastructure.Authorization
{
    public class CurrentUser(string userID, string Email, string RoleName)
    {
        public string UserID = userID;  

        public string EmailAddress = Email;

        public string RoleName = RoleName;
      
        public bool IsInRole(string roleName)
        {
            return RoleName.Contains(roleName);
        }
    }
}
