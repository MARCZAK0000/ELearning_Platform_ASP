namespace ELearning_Platform.Domain.Database.Enitities
{
    public class LessonMaterials
    {
        public string LessonMaterialID { get; set; } = Guid.NewGuid().ToString();

        public string Name { get; set; }

        public string Type { get; set; }

        public virtual Lesson Lesson { get; set; }

        public string LessonID { get; set; }
    }
}
