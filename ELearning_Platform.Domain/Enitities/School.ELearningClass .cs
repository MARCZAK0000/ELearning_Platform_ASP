namespace ELearning_Platform.Domain.Enitities
{
    public class ELearningClass
    {
        public Guid ELearningClassID { get; set; }

        public string Name { get; set; }    

        public int YearOfBeggining { get; set; }

        public int YearOfEnding { get; set; }

        public virtual List<Subject>? Subjects { get; set; }

        public virtual List<UserInformations>? Students { get; set; }

        public virtual List<Lesson>? Lessons { get; set; }

        public virtual List<UserInformations>? Teachers { get; set; }

        public DateTime ModifiedDate { get; set; } = DateTime.Now;
    }
}
