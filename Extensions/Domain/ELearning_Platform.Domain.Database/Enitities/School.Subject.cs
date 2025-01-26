namespace ELearning_Platform.Domain.Database.Enitities
{
    public class Subject
    {
        public string SubjectId { get; set; }

        public string ClassID { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public string TeacherID { get; set; }

        public List<Grade>? Grades { get; set; }

        public List<StudentSubject>? Students { get; set; }

        public UserInformations Teacher { get; set; }

        public ELearningClass Class { get; set; }

        public List<Lesson>? Lessons { get; set; }

        public List<Test>? Tests { get; set; }

        public DateTime ModifiedDate { get; set; } = DateTime.Now;
    }
}
