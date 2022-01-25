using System;
using System.Collections.Generic;

#nullable disable

namespace ofisprojesi
{
    public partial class Debit
    {
        public int Id { get; set; }
        public int? EmployeeId { get; set; }
        public DateTime? CreatedDate { get; set; }
        public int? FixtureId { get; set; }
        public DateTime? FinishDate { get; set; }

        public virtual Employee Employee { get; set; }
        public virtual Fixture Fixture { get; set; }
    }
}
