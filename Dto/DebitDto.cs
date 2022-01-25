using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
namespace ofisprojesi
{
    public class DebitDto
    {
        public int? Id { get; set; }
        public int? EmployeeId { get; set; }
        public String EmployeeName { get; set; }
        public string EmployeeLastname { get; set; }
        public int? FixtureId { get; set; }
        public String FixtureName { get; set; }
        public DateTime? Created_Date { get; set; }
        public DateTime? Finish_Date { get; set; }
    }
    public class DebitSaveDto
    {
        public int? EmployeeId { get; set; }
        public int? FixtureId { get; set; }
    }
    public class DebitSearchDto
    {
        public DateTime? Start_Date { get; set; }
        public DateTime? End_Date { get; set; }
    }
}