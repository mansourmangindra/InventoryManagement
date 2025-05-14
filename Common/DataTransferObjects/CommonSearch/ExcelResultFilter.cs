using Common.DataTransferObjects.Filter;
using System.Text;

namespace Common.DataTransferObjects.Application.CommonSearch
{
    public class ExcelResultFilter : KeywordActivePagination
    {
        public int RoleId { get; set; }
        public string TransactionBy { get; set; }

        public override string GetQueryString()
        {
            StringBuilder sb = new(base.GetQueryString());

            sb.Append(QueryFilter(RoleId, $"&RoleId="));
            sb.Append(QueryFilter(TransactionBy, $"&TransactionBy="));

            return sb.ToString();
        }
    }
}