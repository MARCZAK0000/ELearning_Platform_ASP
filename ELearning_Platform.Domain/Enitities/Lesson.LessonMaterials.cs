namespace ELearning_Platform.Domain.Enitities
{
    public class LessonMaterials
    {
        public Guid LessonMaterialID { get; set; }

        public string Name { get; set; }

        public string Type { get; set; }

        public virtual Lesson Lesson { get; set; }

        public Guid LessonID { get; set; }
    }
}
