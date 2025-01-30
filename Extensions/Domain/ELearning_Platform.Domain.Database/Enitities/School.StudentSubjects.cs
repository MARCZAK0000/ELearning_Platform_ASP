namespace ELearning_Platform.Domain.Database.Enitities
{
    public class StudentSubject
    {
        public int StudentSubjectId { get; set; }

        public string StudentID { get; set; }

        public string SubjectID { get; set; }

        public Subject Subject { get; set; }

        public User Student { get; set; }
    }
}
