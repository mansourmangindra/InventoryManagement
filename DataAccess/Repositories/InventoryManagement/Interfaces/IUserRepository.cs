using DataAccess.DbContexts.InventoryManagement.Models;
using DataAccess.Repositories.Base;

namespace DataAccess.Repositories.InventoryManagement.Interfaces
{
    public interface IUserRepository : IBaseRepository<User>
    {
        Task<User> CreateOrUpdateUserAsync(int action, int? userId, string name, string phoneNumber, string emailAddress, string password, string createdBy, string updatedBy, bool active);
    }
}