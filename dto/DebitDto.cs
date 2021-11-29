using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
namespace Ofisprojesi
{
    public class DebitDto
    {
        public int? Id{get;set;}
        public int? EmployeeId { get; set; }
        public String EmployeeName{get;set;}
        public DateTime? date { get; set; }
        public bool? status { get; set; }
        public int? FixtureId { get; set; }
        public string FixtureName{get;set;}
    }
    public class DebitSaveDto{
        
        public int? EmployeeId{get;set;}
        public DateTime date{get;set;}
        public int FixtureId{get;set;}
            }
}