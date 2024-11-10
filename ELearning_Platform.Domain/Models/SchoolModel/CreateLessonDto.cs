using ELearning_Platform.Domain.Enitities;
using Microsoft.AspNetCore.Http;

namespace ELearning_Platform.Domain.Models.SchoolModel
{
    public class CreateLessonDto
    {
        public string ClassID { get; set; }

        public string SubjectID { get; set; }

        public string LessonName { get; set; }

        public string LessonDescription { get; set; }

        public DateOnly LessonDate {  get; set; }   

        public List<IFormFile>? Materials { get; set; }
    }
}
