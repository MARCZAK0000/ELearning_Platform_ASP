using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ELearning_Platform.Domain.Models.Pagination
{
    public class PaginationModelDto
    {
        public int PageIndex { get; set; }

        public int PageSize { get; set; }
    }
}
