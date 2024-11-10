using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ELearning_Platform.Domain.Enitities
{
    public class Notification
    {
        public Guid NotficaitonID { get; set; }

        public string Title { get; set; }   

        public string Description { get; set; }

        public string? RecipientID { get; set; }

        public UserInformations? Recipient {  get; set; }

        public string? SenderID { get; set; }

        public UserInformations? Sender { get; set; }

        public DateTime TimeSent { get; set; } = DateTime.Now;   

        public bool IsUnread { get; set; } = true;

        
    }
}
