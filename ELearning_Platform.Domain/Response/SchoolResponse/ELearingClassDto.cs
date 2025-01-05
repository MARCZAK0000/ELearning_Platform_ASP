using ELearning_Platform.Domain.Enitities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ELearning_Platform.Domain.Response.SchoolResponse
{
    public class ELearingClassDto
    {
        public string ELearningClassID { get; set; } = Guid.NewGuid().ToString();

        public string Name { get; set; }

        public int YearOfBeggining { get; set; }

        public int YearOfEnding { get; set; }

        public List<SubjectInformationsDto>? Subjects { get; set; }

        public List<StudentInformationsDto>? Students { get; set; }

        public List<TeacherInfromationsDto>? Teachers { get; set; }

        public DateTime ModifiedDate { get; set; } 

    }
}
