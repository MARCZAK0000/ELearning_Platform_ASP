using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ELearning_Platform.Domain.Response.SchoolResponse
{
    public class StudentInformationsDto
    {
        public string AccountID { get; set; }

        public string FirstName { get; set; }

        public string? SecondName { get; set; }

        public string Surname { get; set; }
    }
}
