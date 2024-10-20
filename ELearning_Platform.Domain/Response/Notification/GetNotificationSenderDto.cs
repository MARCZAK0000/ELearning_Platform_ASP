namespace ELearning_Platform.Domain.Response.Notification
{
    public class GetNotificationSenderDto
    {
        public string AccountID { get; set; }

        public string FirstName { get; set; }

        public string Surname { get; set; }

        public string? Email { get; set; }
    }
}
