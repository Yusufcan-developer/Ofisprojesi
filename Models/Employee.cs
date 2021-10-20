using System;
using System.Collections.Generic;

#nullable disable

namespace ofisprojesi
{
    public partial class Employee
    {
        public Employee()
        {
            Debits = new HashSet<Debit>();
        }

        public int Id { get; set; }
        public string Name { get; set; }
        public string Lastname { get; set; }
        public bool? Status { get; set; }
        public int? OfficeId { get; set; }
        public DateTime? RecordDate { get; set; }
        public DateTime? UpdateDate { get; set; }
        public int? Age { get; set; }

        public virtual Office Office { get; set; }
        public virtual EmployeeMovement EmployeeMovement { get; set; }
        public virtual ICollection<Debit> Debits { get; set; }
    }
}
