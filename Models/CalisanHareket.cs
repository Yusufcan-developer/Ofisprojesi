using System;
using System.Collections.Generic;

#nullable disable

namespace ofisprojesi
{
    public partial class CalisanHareket
    {
        public int Id { get; set; }
        public string Adi { get; set; }
        public string Soyad { get; set; }
        public DateTime? Tarih { get; set; }
        public int? Ofis { get; set; }
    }
}
