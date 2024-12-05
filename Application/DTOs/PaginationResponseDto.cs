using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XMP.Application.DTOs
{
    public class PaginationResponseDto<T>
    {
        public IEnumerable<T> Items { get; set; }
        public int TotalCount { get; set; }
        public int Start { get; set; }
        public int End { get; set; }
    }
}
