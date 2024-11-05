namespace ELearning_Platform.Domain.Models.ELearningTestModel
{
    public class CreateQuestionsDto
    {
        public string QuestionText { get; set; }

        public int CorrectAnswerIndex { get; set; }

        public List<CreateAnswerDto> Answers { get; set; }
    }

    public class CreateAnswerDto
    {
        public string AnswerText { get; set; }
    }
}
