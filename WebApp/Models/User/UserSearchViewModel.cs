using Common.DataTransferObjects.CommonSearch;
using Common.DataTransferObjects.Filter.CollectionPaging;
using Common.DataTransferObjects.User;

namespace WebApp.Models.User
{
    public class UserSearchViewModel
    {
        public UserSearchFilter UserSearchFilter { get; set; }
        public PagedList<UserDetail> UserDetails { get; set; }
    }
}
