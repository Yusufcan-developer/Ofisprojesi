using System;
using System.Collections.Generic;

#nullable disable

namespace ofisprojesi
{
    public partial class Calisan
    {
        public Calisan()
        {
            Zimmets = new HashSet<Zimmet>();
        }

        public int Id { get; set; }
        public string Ad { get; set; }
        public string Soyad { get; set; }
        public bool Durum { get; set; }
        public int BagliOlduguOfis { get; set; }

        public virtual Ofi BagliOlduguOfisNavigation { get; set; }
        public virtual ICollection<Zimmet> Zimmets { get; set; }
    }
}
