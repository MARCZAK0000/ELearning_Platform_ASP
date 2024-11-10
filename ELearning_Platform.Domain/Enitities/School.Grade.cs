namespace ELearning_Platform.Domain.Enitities
{
    public class Grade
    {
        public Guid GradeID { get; set; }

        public Guid SubjectID { get; set; }

        public string StudentID { get; set; }

        public Guid TestID { get; set; }

        public string GradeValue { get; set; }

        public DateTime ModifiedDate { get; set; } = DateTime.Now;

        public Subject Subject { get; set; }

        public UserInformations Student { get; set; }

        public Test Test { get; set; }
    }
}
