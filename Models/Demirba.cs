using System;
using System.Collections.Generic;

#nullable disable

namespace ofisprojesi
{
    public partial class Demirba
    {
        public Demirba()
        {
            Zimmets = new HashSet<Zimmet>();
        }

        public int Demirbasid { get; set; }
        public string DemirbasAdi { get; set; }
        public bool? DemirbasDurumu { get; set; }
        public int? DemirbasOda { get; set; }

        public virtual ICollection<Zimmet> Zimmets { get; set; }
    }
}
