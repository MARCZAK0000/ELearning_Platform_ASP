using ELearning_Platform.Domain.Models.Models.Pagination;

namespace ELearning_Platform.Domain.Models.Models.SchoolModel
{
    public class GetLessonsBySubjectIDDto
    {
        public string SubjectID { get; set; }

        public PaginationModelDto Pagination { get; set; }
    }
}
