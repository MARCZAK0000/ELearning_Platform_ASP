using ELearning_Platform.Domain.Models.Pagination;

namespace ELearning_Platform.Domain.Models.SchoolModel
{
    public class GetLessonsBySubjectIDDto
    {
        public string SubjectID {  get; set; }  
        
        public PaginationModelDto Pagination { get; set; }
    }
}
