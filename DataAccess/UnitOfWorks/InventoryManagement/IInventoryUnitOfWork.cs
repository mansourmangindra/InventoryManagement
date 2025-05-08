using DataAccess.Repositories.InventoryManagement;
using DataAccess.Repositories.InventoryManagement.Interfaces;
using DataAccess.UnitOfWorks._Base;

namespace DataAccess.UnitOfWorks.InventoryManagement
{
    public interface IInventoryUnitOfWork : IBaseUnitOfWork
    {
        public IErrorLogRepository ErrorLogRepository { get; }
        public IUserRepository UserRepository { get; }
        public IUserRoleRepository UserRoleRepository { get; }
        public IRoleRepository RoleRepository { get; }
        
        
    }
}
