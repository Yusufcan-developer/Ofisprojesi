using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Ofisprojesi
{
    public class ResultDto
    {
        public int PageIndex { get; set; }
        public int PageCount { get; set; }
        public int TotalDataCount { get; set; }
        public int TotalPages { get { return (int)Math.Ceiling((decimal)TotalDataCount / (decimal)PageCount); } }
        public bool page { get; set; }

    }
}