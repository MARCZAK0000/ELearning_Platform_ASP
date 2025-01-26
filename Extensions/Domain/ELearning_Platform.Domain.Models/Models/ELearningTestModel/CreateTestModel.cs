using ELearning_Platform.Domain.Models.Models.SchoolModel;

namespace ELearning_Platform.Domain.Models.Models.ELearningTestModel
{
    public class CreateTestModel
    {
        public string? TeacherID { get; set; }

        public string SubjectID { get; set; }

        public string ClassID { get; set; }

        public string TestName { get; set; }

        public string TestDescription { get; set; }

        public TestLevel TestLevel { get; set; }

        public DateTime StartTime { get; set; }

        public DateTime EndTime { get; set; }

        public List<CreateQuestionsDto> Questions { get; set; }
    }
}
