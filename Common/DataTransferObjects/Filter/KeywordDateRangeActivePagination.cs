using System.Text;

namespace Common.DataTransferObjects.Filter
{
    public class KeywordDateRangeActivePagination : KeywordActivePagination
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