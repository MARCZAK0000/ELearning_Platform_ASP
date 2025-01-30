using ELearning_Platform.Domain.Database.EntitiesMapper;

namespace ELearning_Platform.Domain.Database.EntitiesDto
{
    public class ElearningClassDto
    {
        public string ElearningClassID { get; set; }

        public string Name { get; set; }

        public int YearOfBeggining { get; set; }

        public int YearOfEnding { get; set; }

        public List<StudentsDto>? Students { get; set; }

        public List<TeacherDto>? Teachers { get; set; }

        public List<SubjectDto>? Subjects { get; set; }


    }
}
