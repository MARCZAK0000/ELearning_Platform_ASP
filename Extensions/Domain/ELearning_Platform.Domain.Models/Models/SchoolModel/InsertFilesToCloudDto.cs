namespace ELearning_Platform.Domain.Models.Models.SchoolModel
{
    public class InsertFilesToCloudDto
    {
        public string LessonId { get; set; }

        public List<MaterialFiles> Materials { get; set; }
    }
}
