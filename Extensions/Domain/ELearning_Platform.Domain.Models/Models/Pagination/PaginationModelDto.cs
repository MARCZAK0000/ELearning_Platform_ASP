using ELearning_Platform.Domain.Models.Order;

namespace ELearning_Platform.Domain.Models.Models.Pagination
{
    public class PaginationModelDto
    {
        public int PageIndex { get; set; }

        public int PageSize { get; set; }

    }


    public class PaginationOrderModelDto<T> : PaginationModelDto where T : Enum
    {
        public OrderByEnum OrderBy { get; set; }

        public bool IsDesc { get; set; }
    }
}
