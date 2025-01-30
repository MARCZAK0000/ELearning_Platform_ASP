namespace ELearning_Platform.Domain.Database.Enitities
{
    public class ELearningClass
    {
        public string ELearningClassID { get; set; } = Guid.NewGuid().ToString();

        public string Name { get; set; }

        public int YearOfBeggining { get; set; }

        public int YearOfEnding { get; set; }

        public List<Subject>? Subjects { get; set; }

        public List<Students>? Students { get; set; }

        public List<Lesson>? Lessons { get; set; }

        public List<Teachers>? Teachers { get; set; }

        public DateTime ModifiedDate { get; set; } = DateTime.Now;
    }
}
