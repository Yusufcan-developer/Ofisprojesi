using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ofisprojesi
{
    public class EmployeeDto
    {
        public int? Id { get; set; }
        public string Name { get; set; }
        public string Lastname { get; set; }
        public bool? Status { get; set; }
        public int? OfficeId { get; set; } 
        public DateTime? RecordDate { get; set; }  
        public DateTime? UpdateDate { get; set; }
        public int? Age { get; set; }

        public Ofisprojesi.DebitDto[] Debiting{get; set;}
    }
}