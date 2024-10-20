using ELearning_Platform.Domain.Enitities;

namespace ELearning_Platform.Domain.Response.Notification
{
    public class GetNotificationModelDto
    {
        public Guid NotificationID { get; set; }

        public string ReciverID {  get; set; }

        public string Title { get; set; }

        public string Description { get; set; }

        public GetNotificationSenderDto Sender { get; set; }

        public bool IsUnRead { get; set; }

        public DateTime TimeSent { get; set; }
    }
}
