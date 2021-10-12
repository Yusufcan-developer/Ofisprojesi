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

        public int Calisanid { get; set; }
        public string CalisanAdi { get; set; }
        public string CalisanSoyadi { get; set; }
        public bool? CalisanDurum { get; set; }
        public int? CalisanOda { get; set; }

        public virtual ICollection<Zimmet> Zimmets { get; set; }
    }
}
