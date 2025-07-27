using Common.DataTransferObjects.CommonSearch;
using Common.DataTransferObjects.Filter;
using Common.DataTransferObjects.Filter.CollectionPaging;
using Common.DataTransferObjects.Role;
using Common.DataTransferObjects.UserRole;
using DataAccess.DbContexts.InventoryManagement.Models;
using DataAccess.Repositories.Base;

namespace DataAccess.Repositories.InventoryManagement.Interfaces
{
    public interface IRoleRepository : IBaseRepository<Role>
    {
        Task<IEnumerable<Role>> GetRoleAllFromSP();
        Task<IEnumerable<UserRoleDetail>> GetRoleAllFromSPJoin();
        Task<PagedList<Role>> GetRoleAllFromSPPagedListAsync(BasicSearchFilter filter);

        Task<PagedList<UserRoleDetail>> GetRoleAllFromSPJoinPagedList(KeywordDateRangeActivePagination filter);
        Task<Role> CreateRoleAsync(int roleId, string name, string createdBy, string updatedBy, bool active = true);
        Task<Role> UpdateRoleAsync(int roleId, string name, string updatedBy, bool active = true);
    }
}