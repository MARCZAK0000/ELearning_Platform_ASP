namespace ELearning_Platform.Domain.Enitities
{
    public class UserAnswers
    {
        public int UserAnswerID { get; set; }

        public Guid? QuestionID { get; set; }

        public Guid TestID { get; set; }

        public Guid? AnswerID { get; set; }

        public string UserID { get; set; }

        public Guid? GradeID { get; set; }

        public UserInformations User {  get; set; }

        public Test Test { get; set; }

        public Questions Question { get; set; }

        public Answers Answers { get; set; }
    }
}
