namespace ELearning_Platform.Domain.Models.Response.Pagination
{
    public class Pagination<T> where T : class
    {
        public List<T> Items { get; set; }

        public int FirstIndex { get; set; }

        public int LastIndex { get; set; }

        public int PageIndex { get; set; }

        public int PageSize { get; set; }

        public int TotalCount { get; set; }
    }
}
