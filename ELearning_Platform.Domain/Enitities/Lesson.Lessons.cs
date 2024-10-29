namespace ELearning_Platform.Domain.Enitities
{
    public class Lesson
    {
        public Guid LessonID { get; set; }

        public string LessonTopic { get; set; }

        public string LessonDescription { get; set; }

        public virtual UserInformations Teacher { get; set; }

        public string TeacherID { get; set; }

        public virtual List<LessonMaterials>? LessonMaterials { get; set; }

        public Guid ClassID { get; set; }

        public virtual ELearningClass Class { get; set; }

        public virtual Subject Subject { get; set; } 

        public Guid SubjectID { get; set; } 

        public DateOnly LessonDate { get; set; }
    }
}
