using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ofisprojesi
{
    public class Employeedto
    {
        public int? Id{get;set;}
        public string Name { get; set;}
        public string Lastname{get;set;}
        public int Age{get;set;}
        public bool Status{get;set;}
        public string dami{get{return "Dummy";}}
    }
}