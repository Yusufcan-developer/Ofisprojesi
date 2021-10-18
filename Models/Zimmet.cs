using System;
using System.Collections.Generic;

#nullable disable

namespace ofisprojesi
{
    public partial class Zimmet
    {
        public int Id { get; set; }
        public int? CalisanId { get; set; }
        public DateTime? Tarih { get; set; }
        public bool? Durum { get; set; }
        public int? DemirbasId { get; set; }
        public string CalisanAd { get; set; }
        public string DemirbasAd { get; set; }

        public virtual Calisan Calisan { get; set; }
        public virtual Demirba Demirbas { get; set; }
    }
}
