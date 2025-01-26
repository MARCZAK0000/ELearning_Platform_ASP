namespace ELearning_Platform.Domain.Models.Models.ELearningTestModel
{
    public class DoTestModelDto
    {
        public string TestID { get; set; }

        public List<DoTestQuestionDto> TestQuestions { get; set; }
    }
}
