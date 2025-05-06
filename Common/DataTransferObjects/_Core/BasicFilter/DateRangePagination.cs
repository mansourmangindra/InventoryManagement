using Common.DataTransferObjects._Core.CollectionPaging;
using System.Text;

namespace Common.DataTransferObjects._Core.BasicFilter
{
    public class DateRangePagination : PagingParameter
    {
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }

        public override string GetQueryString()
        {
            StringBuilder sb = new(base.GetQueryString());
            sb.Append(QueryFilter(StartDate, $"&StartDate="));
            sb.Append(QueryFilter(EndDate, $"&EndDate="));
            return sb.ToString();
        }
    }
}
