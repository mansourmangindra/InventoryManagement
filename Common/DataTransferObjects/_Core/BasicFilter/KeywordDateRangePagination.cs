using System.Text;

namespace Common.DataTransferObjects._Core.BasicFilter
{
    public class KeywordDateRangePagination : DateRangePagination
    {
        public string Keyword { get; set; }

        public override string GetQueryString()
        {
            StringBuilder sb = new(base.GetQueryString());
            sb.Append(QueryFilter(Keyword, $"&Keyword="));
            return sb.ToString();
        }
    }
}
