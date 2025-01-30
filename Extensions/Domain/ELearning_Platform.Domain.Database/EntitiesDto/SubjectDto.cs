namespace ELearning_Platform.Domain.Database.EntitiesDto
{
    public class SubjectDto
    {
        public string SubjectID { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public List<LessonDto>? Lessons { get; set; }

        public List<TestDto> Tests {  get; set; } 
    }
}
