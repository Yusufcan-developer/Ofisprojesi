﻿using System;
using System.Collections.Generic;

#nullable disable

namespace ofisprojesi.Models
{
    public partial class Demirba
    {
        public int Demirbasid { get; set; }
        public string DemirbasAdi { get; set; }
        public bool? DemirbasDurumu { get; set; }
        public int? DemirbasOda { get; set; }
    }
}
