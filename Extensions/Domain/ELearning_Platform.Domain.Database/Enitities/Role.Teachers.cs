using System.ComponentModel.DataAnnotations.Schema;

namespace ELearning_Platform.Domain.Database.Enitities
{
    [Table("Teachers", Schema = "Role")]
    public class Teachers
    {
        public string AccountID { get; set; }

        public string Title { get; set; }

        public bool IsHeadTeachers { get; set; }

        public User User { get; set; }

        public List<ELearningClass> TeachingClass { get; set; }

        public List<Subject> TeachingSubjects { get; set; }

        public List<Lesson> TeachingLessons { get; set; }
    }
}
