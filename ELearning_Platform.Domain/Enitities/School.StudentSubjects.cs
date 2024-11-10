namespace ELearning_Platform.Domain.Enitities
{
    public class StudentSubject
    {
        public int StudentSubjectId { get; set; } 

        public string StudentID { get; set; }   

        public Guid SubjectID { get; set; }

        public Subject Subject { get; set; }

        public UserInformations Student {  get; set; }
    }
}
