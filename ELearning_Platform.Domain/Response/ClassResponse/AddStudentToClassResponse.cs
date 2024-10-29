using ELearning_Platform.Domain.Enitities;

namespace ELearning_Platform.Domain.Response.ClassResponse
{
    public class AddStudentToClassResponse
    {
        public bool IsSuccess { get; set; }

        public string ClassName {  get; set; }  

        public List<UserInformations> AddedUsers { get; set; }
    }
}
