namespace ELearning_Platform.Domain.Database.Enitities
{
    public partial class User
    {
        public string AccountID { get; set; }

        public Account Account { get; set; }

        public string FirstName { get; set; }

        public string? SecondName { get; set; }

        public string Surname { get; set; }

        public string EmailAddress { get; set; }

        public string PhoneNumber { get; set; }

        public DateTime ModifiedDate { get; set; } = DateTime.Now;

        public virtual UserAddress Address { get; set; }

        public List<Students> Students { get; set; }

        public List<Teachers> Teachers { get; set; }

        public virtual List<Notification>? SentNotfications { get; set; }

        public virtual List<Notification>? RecivedNotifications { get; set; }
    }
}
