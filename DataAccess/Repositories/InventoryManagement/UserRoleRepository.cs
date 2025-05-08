
using DataAccess.DbContexts.InventoryManagement;
using DataAccess.DbContexts.InventoryManagement.Models;
using DataAccess.Repositories.Base;
using DataAccess.Repositories.InventoryManagement.Interfaces;

namespace DataAccess.Repositories.InventoryManagement
{
    public class UserRoleRepository : BaseRepository<UserRole>, IUserRoleRepository
    {
        public UserRoleRepository(InventoryManagementDbContext context) : base(context)
        {

        }
    }
}