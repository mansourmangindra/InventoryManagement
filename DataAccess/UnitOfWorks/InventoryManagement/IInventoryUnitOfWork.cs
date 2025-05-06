
using DataAccess.Repositories.DotNet8.Interfaces;
using DataAccess.Repositories.InventoryManagement.Interfaces;
using DataAccess.UnitOfWorks._Base;

namespace DataAccess.UnitOfWorks.InventoryManagement
{
    public interface IInventoryUnitOfWork : IBaseUnitOfWork
    {
        public IErrorLogRepository ErrorLogRepository { get; }
        public IUserRepository UserRepository { get; }
        
    }
}
