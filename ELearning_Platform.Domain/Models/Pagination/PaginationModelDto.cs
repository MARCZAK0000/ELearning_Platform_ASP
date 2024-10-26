using ELearning_Platform.Domain.Order;

namespace ELearning_Platform.Domain.Models.Pagination
{
    public class PaginationModelDto
    {
        public int PageIndex { get; set; }

        public int PageSize { get; set; }

        public OrderByEnum OrderBy { get; set; }

        public bool IsDesc {  get; set; }
    }
}
