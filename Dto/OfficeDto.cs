using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ofisprojesi;
using Ofisprojesi;

namespace ofisprojesi
{
    public class OfficeDto
    {
        public int? Id{get;set;}
        public bool? Status{get;set;}
        public string Name{get;set;}
        public DateTime recdate{get;set;}
        public DateTime update{get;set;}
        public virtual FixtureDto[] employee { get; set; }
        public virtual FixtureDto[] Fixtures { get; set; }
    }

    public class OfficeUpdateDto
    {
        public string Name { get; set; }
    }
}