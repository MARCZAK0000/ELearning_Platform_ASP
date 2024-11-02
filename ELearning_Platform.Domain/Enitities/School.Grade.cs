using ELearning_Platform.Domain.Models.SchoolModel;

namespace ELearning_Platform.Domain.Enitities
{
    public class Grade
    {
        public Guid GradeID {  get; set; }

        public Guid SubjectID { get; set; }

        public string AccountId {  get; set; }

        public Guid TestID { get; set; }    

        public string GradeValue { get; set; }

        public TestLevel GradeLevel { get; set; }

        public DateTime ModifiedDate { get; set; } = DateTime.Now;

        public Subject Subject { get; set; }

        public UserInformations Account { get; set; }

        public Test Test { get; set; }
    }
}
