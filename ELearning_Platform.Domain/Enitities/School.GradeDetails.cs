namespace ELearning_Platform.Domain.Enitities
{
    public class GradeDetails
    {
        public string GradeID { get; set; }

        public int GradePointsScore { get; set; } = 0;

        public int TestQuestionsCount { get; set; } = 0;

        public decimal GradePercentageScore { get 
            {
                if (TestQuestionsCount == 0) return 0;
                return (GradePointsScore / TestQuestionsCount) * 100;
            } 
        }

        public Grade Grade { get; set; }
    }
}
