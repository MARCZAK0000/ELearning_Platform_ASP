namespace ELearning_Platform.Domain.Models.Response.Pagination
{
    public class PaginationBuilder<T> where T : class
    {
        private Pagination<T> _response = new Pagination<T>();


        public Pagination<T> Build()
        {
            return _response;
        }

        public PaginationBuilder<T> SetItems(List<T> items)
        {
            _response.Items = items;
            return this;
        }
        public PaginationBuilder<T> SetTotalCount(int totalCount)
        {
            _response.TotalCount = totalCount;
            return this;
        }
        public PaginationBuilder<T> SetPageSize(int pageSize)
        {
            _response.PageSize = pageSize;
            return this;
        }
        public PaginationBuilder<T> SetPageIndex(int pageIndex)
        {
            _response.PageIndex = pageIndex;
            return this;
        }
        public PaginationBuilder<T> SetFirstIndex(int pageSize, int pageIndex)
        {
            _response.FirstIndex = (pageIndex - 1) * pageSize + 1;
            return this;
        }
        public PaginationBuilder<T> SetLastIndex(int pageSize, int pageIndex)
        {
            _response.LastIndex = (pageIndex - 1) * pageSize + pageSize;
            return this;
        }
    }
}
