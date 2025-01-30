
using ELearning_Platform.Domain.Database.Enitities;

namespace ELearning_Platform.Domain.Models.Response.ClassResponse
{
    public class AddStudentToClassResponse
    {
        public bool IsSuccess { get; set; }

        public string ClassName { get; set; }

        public List<User> AddedUsers { get; set; }
    }
}
