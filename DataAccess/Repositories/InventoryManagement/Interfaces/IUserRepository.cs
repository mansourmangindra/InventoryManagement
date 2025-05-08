using DataAccess.DbContexts.InventoryManagement.Models;
using DataAccess.Repositories.Base;

namespace DataAccess.Repositories.InventoryManagement.Interfaces
{
    public interface IUserRepository : IBaseRepository<User>
    {
    }
}