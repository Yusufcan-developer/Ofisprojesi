using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Ofisprojesi
{   public class UserSearchCriteria : BaseSearchCriteria
    {
        public String[] RoleNames { get; set; }
        public bool? IsActive { get; set; }
    }

    public class BasicSearchCriteria : BaseSearchCriteria
    {
        /// <summary>
        /// Tarih - başlangıç
        /// </summary>
        /// <value></value>
        public DateTime? From { get; set; }
        /// <summary>
        /// Tarih - bitiş
        /// </summary>
        /// <value></value>
        public DateTime? To { get; set; }

        public BasicSearchCriteria()
        {
        }
    }

    public class BaseSearchCriteria
    {
        public string Keyword { get; set; }
        public int? PageIndex { get; set; }
        public int? PageCount { get; set; }
        public string SortingField { get; set; }

        public BaseSearchCriteria()
        {
            this.PageIndex = 0;
            this.PageCount = DefaultPageCount;
        }

        public const int DefaultPageCount = 20;
    }
}