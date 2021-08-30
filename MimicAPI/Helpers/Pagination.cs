using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MimicAPI.Helpers
{
    public class Pagination
    {
        public int PageNumber { get; set; }
        public int RecordsPage { get; set; }
        public int TotalRecords { get; set; }
        public int TotalPages { get; set; }
    }
}
