
using DataAccess.DbContexts.InventoryManagement;
using DataAccess.DbContexts.InventoryManagement.Models;
using DataAccess.Repositories.Base;
using DataAccess.Repositories.DotNet8.Interfaces;

namespace DataAccess.Repositories.DotNet8
{
    public class UserRepository : BaseRepository<User>, IUserRepository
    {
        public UserRepository(InventoryManagementDbContext context) : base(context)
        {

        }
    }
}