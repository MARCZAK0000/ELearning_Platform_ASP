namespace ELearning_Platform.Domain.Models.SchoolModel
{
    public class AddStudentToClassDto
    {
        public Guid ClassID { get; set; }

        public List<string> UsersToAdd { get; set; }
    }
}
