using ELearning_Platform.Domain.Models.Models.Pagination;

namespace ELearning_Platform.Domain.Models.Models.ELearningTestModel
{
    public class FindTestsBySomeIDDto
    {
        public string ID { get; set; }

        public bool IsComplited { get; set; }

        public PaginationModelDto PaginationModelDto { get; set; }
    }
}
