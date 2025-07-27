using Common.DataTransferObjects.Filter.CollectionPaging;
using System.Text;

namespace Common.DataTransferObjects.CommonSearch
{
    public class BasicSearchFilter : PagingParameter
    {
        public string Keyword { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }

        public string SearchKeyword { get; set; }

        public IEnumerable<bool> Active { get; set; } = Enumerable.Empty<bool>();

        public string SortOrder { get; set; } = "asc"; // default to 'asc'
        public int SortBy { get; set; } = 0;           // default to first column

        public override string GetQueryString()
        {
            var sb = new StringBuilder();

            sb.Append($"PageNumber={PageNumber}");
            sb.Append($"&PageSize={PageSize}");

            if (StartDate.HasValue)
                sb.Append($"&StartDate={StartDate.Value:yyyy-MM-dd}");

            if (EndDate.HasValue)
                sb.Append($"&EndDate={EndDate.Value:yyyy-MM-dd}");

            if (!string.IsNullOrEmpty(Keyword))
                sb.Append($"&Keyword={Uri.EscapeDataString(Keyword)}");

            if (!string.IsNullOrEmpty(SearchKeyword))
                sb.Append($"&SearchKeyword={Uri.EscapeDataString(SearchKeyword)}");

            if (Active != null && Active.Any())
            {
                foreach (var val in Active)
                    sb.Append($"&Active={val.ToString().ToLower()}");
            }

            if (!string.IsNullOrEmpty(SortOrder))
                sb.Append($"&SortOrder={SortOrder}");

            sb.Append($"&SortBy={SortBy}");

            return sb.ToString();
        }

        public virtual string GetFilterDisplayValue()
        {
            List<string> filterDisplayValues = new();


            if (!String.IsNullOrEmpty(Keyword))
            {
                filterDisplayValues.Add($"KEYWORD={Keyword}");
            }

            if (Active != null && Active.Any())
            {
                string enabledValue = String.Join(',', Active);
                filterDisplayValues.Add($"ACTIVE=[{enabledValue}]".Replace("True", "Yes").Replace("False", "No"));
            }

            if (filterDisplayValues.Any())
            {
                return String.Join(", ", filterDisplayValues);
            }
            else
            {
                return "";
            }
        }
    }
}