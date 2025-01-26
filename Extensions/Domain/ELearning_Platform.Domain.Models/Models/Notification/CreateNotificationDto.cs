namespace ELearning_Platform.Domain.Models.Models.Notification
{
    public class CreateNotificationDto
    {
        public string Title { get; set; }

        public string Describtion { get; set; }

        public string ReciverID { get; set; }

        public string? SenderID { get; set; }

        public string? EmailAddress { get; set; }

    }
}
