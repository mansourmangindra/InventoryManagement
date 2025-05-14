using System.Text;

namespace Common.DataTransferObjects.Filter
{
    public class KeywordActivePagination : ActivePagination
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