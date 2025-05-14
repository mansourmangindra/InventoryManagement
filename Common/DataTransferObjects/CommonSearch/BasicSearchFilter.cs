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

        public IEnumerable<bool> Active { get; set; }

        public override string GetQueryString()
        {
            StringBuilder sb = new();
            
            sb.Append($"PageNumber={PageNumber}");
            sb.Append($"&PageSize={PageSize}");
            sb.Append(QueryFilter(StartDate, $"&StartDate="));
            sb.Append(QueryFilter(EndDate, $"&EndDate="));
            if (!String.IsNullOrEmpty(Keyword))
            {
                sb.Append($"&Keyword={Keyword}");
            }

            if (!String.IsNullOrEmpty(SearchKeyword))
            {
                sb.Append($"&SearchKeyword={SearchKeyword}");
            }

            if (Active != null && Active.Any())
            {
                foreach (var active in Active)
                {
                    sb.Append($"&Active={active}");
                }
            }

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