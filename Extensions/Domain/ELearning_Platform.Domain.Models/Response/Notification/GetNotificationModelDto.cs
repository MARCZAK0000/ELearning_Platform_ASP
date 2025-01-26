namespace ELearning_Platform.Domain.Models.Response.Notification
{
    public class GetNotificationModelDto
    {
        public string NotificationID { get; set; }

        public string ReciverID { get; set; }

        public string Title { get; set; }

        public string Description { get; set; }

        public GetNotificationSenderDto Sender { get; set; }

        public bool IsUnRead { get; set; }

        public DateTime TimeSent { get; set; }
    }
}
