namespace ELearning_Platform.Domain.Database.Enitities
{
    public class Questions
    {
        public string QuestionId { get; set; } = Guid.NewGuid().ToString();

        public string TestId { get; set; }

        public string QuestionText { get; set; }

        public virtual List<Answers> Answers { get; set; }

        public virtual Test Test { get; set; }

        public virtual UserAnswers TestQuestion { get; set; }

    }
}
