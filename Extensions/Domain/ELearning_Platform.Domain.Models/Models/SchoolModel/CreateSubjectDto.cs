namespace ELearning_Platform.Domain.Models.Models.SchoolModel
{
    public class CreateSubjectDto
    {
        public string SubjectName { get; set; }

        public string SubjectDescription { get; set; }

        public string ClassID { get; set; }

        public string? TeacherID { get; set; }
    }
}
