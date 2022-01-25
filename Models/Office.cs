using System;
using System.Collections.Generic;

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

        public int Id { get; set; }
        public string Name { get; set; }
        public bool? Status { get; set; }
        public DateTime? Recdate { get; set; }
        public DateTime? Updatedate { get; set; }

        public virtual ICollection<Employee> Employees { get; set; }
        public virtual ICollection<Fixture> Fixtures { get; set; }
    }
}
