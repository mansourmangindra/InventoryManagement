using Common.DataTransferObjects.Filter;
using System.Text;

namespace Common.DataTransferObjects.Application.CommonSearch
{
    public class BulkUploadSearchFilter : KeywordActivePagination
    {
        public IEnumerable<bool> Valid { get; set; }

        public override string GetQueryString()
        {
            StringBuilder sb = new(base.GetQueryString());

            sb.Append(QueryFilter(Valid, $"&Valid="));

            return sb.ToString();
        }
    }
}