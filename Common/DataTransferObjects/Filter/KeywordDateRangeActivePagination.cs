using System.Text;

namespace Common.DataTransferObjects.Filter
{
    public class KeywordDateRangeActivePagination : KeywordActivePagination
    {
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }

        public IEnumerable<bool> Active { get; set; } = Enumerable.Empty<bool>();
        public string SortOrder { get; set; } = "asc";
        public int SortBy { get; set; } = 0;  


        public override string GetQueryString()
        {
            StringBuilder sb = new(base.GetQueryString());
            sb.Append(QueryFilter(StartDate, $"&StartDate="));
            sb.Append(QueryFilter(EndDate, $"&EndDate="));
            if (!string.IsNullOrEmpty(SortOrder))
                sb.Append($"&SortOrder={SortOrder}");

            if (Active != null && Active.Any())
            {
                foreach (var val in Active)
                    sb.Append($"&Active={val.ToString().ToLower()}");
            }
            sb.Append($"&SortBy={SortBy}");
            return sb.ToString();
        }
    }
}