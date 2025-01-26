using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ELearning_Platform.Domain.Models.Models.UserAddress
{
    public class UpdateAddressDto
    {
        public string City { get; set; }

        public string Country { get; set; }

        public string PostalCode { get; set; }

        public string StreetName { get; set; }
    }
}
