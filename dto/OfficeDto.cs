using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ofisprojesi;

namespace Ofisprojesi
{
    public class OfficeDto
    {
        public int? Id{get;set;}
        public bool? Status{get;set;}
        public string Name{get;set;}
        public virtual EmployeeDto[] employee { get; set; }
        public virtual FixtureDto[] Fixtures { get; set; }
    }
}