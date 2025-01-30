

using ELearning_Platform.Domain.Models.SchoolModel;

namespace ELearning_Platform.Domain.Database.Enitities
{
    public class Test
    {
        public string TestID { get; set; } = Guid.NewGuid().ToString();

        public string? SubjectID { get; set; }

        public string TestName { get; set; }

        public TestLevel TestLevel { get; set; }

        public DateTime StartTime { get; set; }

        public DateTime EndTime { get; set; }

        public bool IsComplited { get; set; } = false;

        public string? TeacherID { get; set; }

        public DateTime ModifiedDate { get; set; } = DateTime.Now;

        public List<Questions>? Questions { get; set; }

        public List<Grade>? Grades { get; set; }

        public Subject Subject { get; set; }

        public User Teacher { get; set; }

        public List<UserAnswers> TestAnswers { get; set; }

        public static string CalcutlateGrade(Func<TestLevel, int, string> calculate, TestLevel testLevel, int score)
        {
            return calculate(testLevel, score);
        }
    }
}
