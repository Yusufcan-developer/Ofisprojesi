using System;
using System.Collections.Generic;

#nullable disable

namespace ofisprojesi
{
    public partial class EmployeeMovement
    {
        public int Id { get; set; }
        public DateTime? Date { get; set; }
        public int? Office { get; set; }

        public virtual Employee IdNavigation { get; set; }
    }
}
