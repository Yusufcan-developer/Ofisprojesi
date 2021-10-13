using System;
using System.Collections.Generic;

#nullable disable

namespace ofisprojesi
{
    public partial class Zimmet
    {
        public int Id { get; set; }
        public int? ZimmetlenenCalisanlar { get; set; }
        public DateTime? Tarih { get; set; }
        public bool? Durum { get; set; }
        public int? ZimmetlenmisDemirbas { get; set; }

        public virtual Calisan ZimmetlenenCalisanlarNavigation { get; set; }
        public virtual Demirba ZimmetlenmisDemirbasNavigation { get; set; }
    }
}
