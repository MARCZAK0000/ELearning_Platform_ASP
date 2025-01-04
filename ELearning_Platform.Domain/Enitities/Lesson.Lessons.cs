namespace ELearning_Platform.Domain.Enitities
{
    public class Lesson
    {
        public string LessonID { get; set; } = Guid.NewGuid().ToString();

        public string LessonTopic { get; set; }

        public string LessonDescription { get; set; }

        public virtual UserInformations Teacher { get; set; }

        public string TeacherID { get; set; }

        public virtual List<LessonMaterials>? LessonMaterials { get; set; }

        public string ClassID { get; set; }

        public virtual ELearningClass Class { get; set; }

        public virtual Subject Subject { get; set; } 

        public string SubjectID { get; set; } 

        public DateOnly LessonDate { get; set; }
    }
}
