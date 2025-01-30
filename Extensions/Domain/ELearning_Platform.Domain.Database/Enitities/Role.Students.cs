using System.ComponentModel.DataAnnotations.Schema;

namespace ELearning_Platform.Domain.Database.Enitities
{
    [Table("Students", Schema = "Role")]
    public class Students
    {
        public string AccountID { get; set; }

        public string ElearningClassID { get; set; }

        public List<Grade> Grades { get; set; }

        public List<StudentSubject> Subjects { get; set; }

        public ELearningClass ElearningClass { get; set; }

        public User User {  get; set; }

        public List<Test>? Tests { get; set; }

        public List<UserAnswers>? UserAnswers { get; set; }

    }
}
