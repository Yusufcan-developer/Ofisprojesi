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

        public int Id { get; set; }
        public string Ad { get; set; }
        public bool Durum { get; set; }
        public int BulunduguOfis { get; set; }

        public virtual Ofi BulunduguOfisNavigation { get; set; }
        public virtual ICollection<Zimmet> Zimmets { get; set; }
    }
}
