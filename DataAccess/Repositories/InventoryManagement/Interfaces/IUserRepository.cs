using DataAccess.DbContexts.InventoryManagement.Models;
using DataAccess.Repositories.Base;

namespace DataAccess.Repositories.DotNet8.Interfaces
{
    public interface IUserRepository : IBaseRepository<User>
    {
    }
}