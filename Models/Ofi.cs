using System;
using System.Collections.Generic;

#nullable disable

namespace ofisprojesi
{
    public partial class Ofi
    {
        public Ofi()
        {
            Calisans = new HashSet<Calisan>();
            Demirbas = new HashSet<Demirba>();
        }

        public int Id { get; set; }
        public string Ad { get; set; }
        public bool? Durum { get; set; }

        public virtual ICollection<Calisan> Calisans { get; set; }
        public virtual ICollection<Demirba> Demirbas { get; set; }
    }
}
