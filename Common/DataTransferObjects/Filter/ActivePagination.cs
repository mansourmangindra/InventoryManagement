
using Common.DataTransferObjects.Filter.CollectionPaging;
using System.Text;

namespace Common.DataTransferObjects.Filter
{
    public class ActivePagination : PagingParameter
    {
        public IEnumerable<bool> Actives { get; set; }

        public override string GetQueryString()
        {
            StringBuilder sb = new(base.GetQueryString());
            sb.Append(QueryFilter(Actives, $"&Actives="));
            return sb.ToString();
        }
    }
}