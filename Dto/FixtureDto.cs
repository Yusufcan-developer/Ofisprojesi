using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ofisprojesi;

namespace Ofisprojesi
{
    public class FixtureDto
    {
        public int id { get; set; }
        public string Name { get; set; }
        public int? Officeid { get; set; }
        public bool? Status { get; set; }
        public DateTime? Recdate{get;set;}
        public DateTime? Updatedate{get;set;}
        public DebitDto[] debits {get;set;}
        
    }
    public class FixtureUpdateDto
    {
        public string name { get; set; }
        public int? Officeid { get; set; }

    }

}