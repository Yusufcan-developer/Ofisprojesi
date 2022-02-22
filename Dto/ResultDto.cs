using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ofisprojesi;

namespace Ofisprojesi
{
    public class ResultDto
    {
        public DateTime? todate {get;set;}
        public DateTime? fromdate {get;set;}
        public int PageIndex { get; set; }
        public int PageCount { get; set; }
        public int TotalDataCount { get; set; }
        public int TotalPages { get { return (int)Math.Ceiling((decimal)TotalDataCount / (decimal)PageCount); } }
        public System.Linq.IQueryable<Ofisprojesi.FixtureDto> Data { get; set; }

    }
    
}