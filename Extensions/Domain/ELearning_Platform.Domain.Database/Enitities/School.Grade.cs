namespace ELearning_Platform.Domain.Database.Enitities
{
    public class Grade
    {
        public string GradeID { get; set; } = Guid.NewGuid().ToString();

        public string SubjectID { get; set; }

        public string StudentID { get; set; }

        public string TestID { get; set; }

        public string GradeValue { get; set; }

        public DateTime ModifiedDate { get; set; } = DateTime.Now;

        public Subject Subject { get; set; }

        public User Student { get; set; }

        public Test Test { get; set; }

        public GradeDetails GradeDetails { get; set; }
    }
}
