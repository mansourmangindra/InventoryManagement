
using DataAccess.DbContexts.InventoryManagement;
using DataAccess.DbContexts.InventoryManagement.Models;
using DataAccess.Repositories.Base;
using DataAccess.Repositories.InventoryManagement.Interfaces;

namespace DataAccess.Repositories.InventoryManagement
{
    public class RoleRepository : BaseRepository<Role>, IRoleRepository
    {
        public RoleRepository(InventoryManagementDbContext context) : base(context)
        {

        }
    }
}