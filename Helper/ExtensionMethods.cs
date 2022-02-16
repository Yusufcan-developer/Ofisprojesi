using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Security.Claims;
using Newtonsoft.Json;
using System.IO;
using System.Text;


namespace Ofisprojesi.helper
{
    public static class ExtensionMethods
    {
        public static IQueryable<T> AddPaging<T>(this IQueryable<T> query, ref int? pageIndex, ref int? pageCount)
        {
            pageIndex = !pageIndex.HasValue || pageIndex < 0 ? 0 : pageIndex;
            pageCount = !pageCount.HasValue || pageCount <= 0 ? 0 : pageCount;
            if (pageIndex > 0)
            {
                query = query.Skip((int)pageIndex * (int)pageCount);
            }
            if (pageCount > 0)
            {
                query = query.Take((int)pageCount);
            }
            return query;
        }

    }
}