using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

#nullable disable

namespace ofisprojesi
{
    public partial class Office
    {
        public Office()
        {
            Employees = new HashSet<Employee>();
            Fixtures = new HashSet<Fixture>();
        }

  
        public int? Id { get; set; }
        public string Name { get; set; }
        public bool? Status { get; set; }

        [System.Text.Json.Serialization.JsonIgnore]
        public virtual ICollection<Employee> Employees { get; set; }
        [System.Text.Json.Serialization.JsonIgnore]
        public virtual ICollection<Fixture> Fixtures { get; set; }
    }
}
