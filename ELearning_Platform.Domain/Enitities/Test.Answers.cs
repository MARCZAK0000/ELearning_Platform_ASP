namespace ELearning_Platform.Domain.Enitities
{
    public class Answers
    {
        public Guid AnswerId {  get; set; }

        public Guid QuestionId { get; set; }

        public string AnswerText { get; set; }

        public bool IsCorrect {  get; set; }

        public virtual Questions Questions { get; set; }

        public UserAnswers UserAnswers { get; set; }
    }
}
