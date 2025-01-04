namespace ELearning_Platform.Domain.Enitities
{
    public class UserAnswers
    {
        public int UserAnswerID { get; set; }

        public string? QuestionID { get; set; }

        public string TestID { get; set; }

        public string? AnswerID { get; set; }

        public string UserID { get; set; }

        public string? GradeID { get; set; }

        public UserInformations User {  get; set; }

        public Test Test { get; set; }

        public Questions Question { get; set; }

        public Answers Answers { get; set; }
    }
}
