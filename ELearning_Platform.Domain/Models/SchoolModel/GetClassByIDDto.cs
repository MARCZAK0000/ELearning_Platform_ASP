using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ELearning_Platform.Domain.Models.SchoolModel
{
    public class GetClassByIDDto
    {
        public string ClassID { get; set; }

        public bool WithStudents { get; set; }

        public bool WithSubjects { get; set; }

        public bool WithTeachers { get; set; }
    }
}
