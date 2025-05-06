using Newtonsoft.Json.Linq;
using System.Text;

namespace Common.DataTransferObjects._Core.CollectionPaging
{
    public class PagingParameter
    {
        const int maxPageSize = 100000;
        private int pageSize = 10;

        public int PageNumber { get; set; } = 1;

        public int PageSize
        {
            get
            {
                return pageSize;
            }
            set
            {
                pageSize = value > maxPageSize ? maxPageSize : value;
            }
        }

        public virtual string GetQueryString()
        {
            StringBuilder sb = new();

            sb.Append(QueryFilter(PageNumber, $"PageNumber="));
            sb.Append(QueryFilter(PageSize, $"&PageSize="));

            return sb.ToString();
        }

        internal static string QueryFilter<TSource>(TSource val, string query)
        {
            StringBuilder sb = new();

            if (!EqualityComparer<TSource>.Default.Equals(val, default(TSource)))
            {
                if (val is string strVal)
                {
                    if (!string.IsNullOrEmpty(strVal) && !string.IsNullOrWhiteSpace(strVal))
                    {
                        sb.Append($"{query}{strVal.Trim()}");
                    }
                }
                else
                {
                    sb.Append($"{query}{val}");
                }
            }
            return sb.ToString();
        }

        internal static string QueryFilter<TSource>(IEnumerable<TSource> val, string query)
        {
            StringBuilder sb = new();
            if (val != null && val.Any())
            {
                foreach (var v in val)
                {
                    sb.Append($"{query}{v}");
                }
            }
            return sb.ToString();
        }
    }
}