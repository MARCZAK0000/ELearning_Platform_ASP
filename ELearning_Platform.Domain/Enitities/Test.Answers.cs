namespace ELearning_Platform.Domain.Enitities
{
    public class Answers
    { 
        public string AnswerId {  get; set; } = Guid.NewGuid().ToString();

        public string QuestionId { get; set; } 

        public string AnswerText { get; set; }

        public bool IsCorrect {  get; set; }

        public virtual Questions Questions { get; set; }

        public UserAnswers UserAnswers { get; set; }
    }
}
