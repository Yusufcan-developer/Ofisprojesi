using System;
using System.Collections.Generic;

#nullable disable

namespace ofisprojesi
{
    public partial class Fixture
    {
        public Fixture()
        {
            Debits = new HashSet<Debit>();
        }

          
        public int? Id { get; set; }
        public string Name { get; set; }
        public bool? Status { get; set; }
        public int? OfficeId { get; set; }

        [System.Text.Json.Serialization.JsonIgnore]
        public virtual Office Office { get; set; }
        [System.Text.Json.Serialization.JsonIgnore]
        public virtual ICollection<Debit> Debits { get; set; }
    }
}
