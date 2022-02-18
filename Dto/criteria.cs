using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Ofisprojesi
{
    public class criteria
    {
            public string SortingField { get; set; }
            public SortingOrder? SortingOrder { get; set; }
            public const int DefaultPageCount = 20;
        }
    }