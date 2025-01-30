using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ELearning_Platform.Domain.Database.Enitities
{
    public class UserAddress
    {
        public string AccountID { get; set; }

        public string City { get; set; }

        public string Country { get; set; }

        public string PostalCode { get; set; }

        public string StreetName { get; set; }

        public DateTime ModifiedDate { get; set; } = DateTime.Now;

        public virtual User User { get; set; }
    }
}
