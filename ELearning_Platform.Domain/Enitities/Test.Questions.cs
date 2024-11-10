namespace ELearning_Platform.Domain.Enitities
{
    public class Questions
    {
        public Guid QuestionId { get; set; }

        public Guid TestId { get; set; }

        public string QuestionText { get; set; }

        public virtual List<Answers> Answers { get; set; }

        public virtual Test Test { get; set; }

        public virtual UserAnswers TestQuestion { get; set; } 

    }
}
