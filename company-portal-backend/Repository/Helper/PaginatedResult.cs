using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository.Helper
{
    public class PaginatedResult<T> where T : class
    {
        public List<T> Items { get; set; } = new();
        public int CurrentPage { get; set; }
        public int PageSize { get; set; }
        public int TotalPages { get; set; }
    }
}
