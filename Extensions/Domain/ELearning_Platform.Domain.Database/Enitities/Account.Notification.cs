namespace ELearning_Platform.Domain.Database.Enitities
{
    public class Notification
    {
        public string NotficaitonID { get; set; } = Guid.NewGuid().ToString();

        public string Title { get; set; }

        public string Description { get; set; }

        public string? RecipientID { get; set; }

        public UserInformations? Recipient { get; set; }

        public string? SenderID { get; set; }

        public UserInformations? Sender { get; set; }

        public DateTime TimeSent { get; set; } = DateTime.Now;

        public bool IsUnread { get; set; } = true;


    }
}
