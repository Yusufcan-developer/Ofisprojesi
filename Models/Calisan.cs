using System;
using System.Collections.Generic;

#nullable disable

namespace ofisprojesi.Models
{
    public partial class Calisan
    {
        public int Calisanid { get; set; }
        public string CalisanAdi { get; set; }
        public string CalisanSoyadi { get; set; }
        public bool calisan_durum { get; set; }
        public int calisan_oda { get; set; }
    }
}
