using System;
using System.Collections.Generic;

#nullable disable

namespace ofisprojesi.Models
{
    public partial class Zimmet
    {
        public int Zimmetid { get; set; }
        public int? ZimmetlenenKisi { get; set; }
        public DateTime? ZimmetlenenTarih { get; set; }
        public bool? ZimmetDurumu { get; set; }
    }
}
