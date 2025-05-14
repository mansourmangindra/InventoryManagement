using System.Text;

namespace Common.DataTransferObjects.CommonSearch
{
    public class UserSearchFilter : BasicSearchFilter
    {
        public IEnumerable<int> RoleIds { get; set; }

        public override string GetQueryString()
        {
            StringBuilder sb = new(base.GetQueryString());

            if (RoleIds != null && RoleIds.Any())
            {
                foreach (var roleId in RoleIds)
                {
                    sb.Append($"&RoleIds={roleId}");
                }
            }

            return sb.ToString();
        }
    }
}
