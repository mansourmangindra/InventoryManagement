using DataAccess.DbContexts.InventoryManagement;
using DataAccess.DbContexts.InventoryManagement.Models;
using DataAccess.Repositories.Base;
using DataAccess.Repositories.InventoryManagement.Interfaces;

namespace DataAccess.Repositories.InventoryManagement
{
    public class ErrorLogRepository : BaseRepository<ErrorLog>, IErrorLogRepository
    {
        public ErrorLogRepository(InventoryManagementDbContext context) : base(context)
        {

        }
    }
}