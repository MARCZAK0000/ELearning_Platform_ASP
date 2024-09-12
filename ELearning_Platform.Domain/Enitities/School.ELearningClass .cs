namespace ELearning_Platform.Domain.Enitities
{
    public class ELearningClass
    {
        public Guid ELearningClassID { get; set; }

        public string Name { get; set; }    

        public decimal YearOfBeggining { get; set; }

        public decimal YearOfEnded { get; set; }

        public virtual List<Subject>? Subjects { get; set; }

        public virtual List<UserInformations>? Students { get; set; }

        public virtual List<Lesson>? Lessons { get; set; }

        public virtual List<UserInformations>? Teachers { get; set; }

        public DateOnly ModifiedDate { get; set; } = DateOnly.FromDateTime(DateTime.Now);
    }
}
