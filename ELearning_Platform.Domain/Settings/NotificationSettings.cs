using System.ComponentModel.DataAnnotations;

namespace ELearning_Platform.Domain.Settings
{
    public class NotificationSettings
    {
        private readonly object _lockA = new();
        private readonly object _lockB = new();
        private readonly object _lockC = new();
        [Required]
        public bool EmailNotification
        {
            get {
                lock (_lockA)
                {
                    return emailNotifications; 
                }
            }
            set { 
                lock (_lockA)
                {
                    emailNotifications = value; 
                }
            }
        }
        private bool emailNotifications;

        [Required]  
        public bool SMSNotification
        {
            get
            {
                lock (_lockB)
                {
                    return smsNotification;
                }
            }
            set
            {
                lock (_lockB)
                {
                    smsNotification = value;
                }
            }
        }
        private bool smsNotification;

        [Required]
      
        public bool PushNotification
        {
            get
            {
                lock (_lockC)
                {
                    return pushNotificationn;
                }
            }
            set
            {
                lock (_lockC)
                {
                    pushNotificationn = value;
                }
            }
        }
        private bool pushNotificationn;
        
    }
}
