namespace ELearning_Platform.Domain.Database.Enitities
{
    public class TeachersClass
    {
        public int TeachersClassID { get; set; }

        public string TeacherID { get; set; }

        public string ClassID { get; set; }

        public List<Teachers> Teachers { get; set; }

        public List<ELearningClass> Classes { get; set; }
    }
}
