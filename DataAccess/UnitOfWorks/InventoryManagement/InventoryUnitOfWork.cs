using Common.DataTransferObjects._Core.AuditTrail;
using DataAccess.DbContexts.InventoryManagement;
using DataAccess.Repositories.InventoryManagement;
using DataAccess.Repositories.InventoryManagement.Interfaces;
using DataAccess.Services.Interfaces;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace DataAccess.UnitOfWorks.InventoryManagement
{
    public sealed class InventoryUnitOfWork : IInventoryUnitOfWork
    {
        private readonly InventoryManagementDbContext _context;
        private readonly IDbContextChangeTrackingService _dbContextChangeTrackingService;
        public InventoryUnitOfWork(InventoryManagementDbContext context, IDbContextChangeTrackingService dbContextChangeTrackingService)
        {
            _context = context;
            _dbContextChangeTrackingService = dbContextChangeTrackingService;
            ErrorLogRepository = new ErrorLogRepository(_context);
            UserRepository = new UserRepository(_context);
            UserRoleRepository = new UserRoleRepository(_context);
            RoleRepository = new RoleRepository(_context);
            UserProfileRepository = new UserProfileRepository(_context);
        }

        public IErrorLogRepository ErrorLogRepository { get; private set; }
        public IUserRepository UserRepository { get; private set; }
        public IUserRoleRepository UserRoleRepository { get; private set; }
        public IRoleRepository RoleRepository { get; private set; }
        public IUserProfileRepository UserProfileRepository { get; private set; }

        public void Dispose()
        {
            _context.Dispose();
        }

        public async Task<int> SaveChangesAsync(string transactionBy)
        {
            List<Tuple<ContextChangeTrackingDetail, EntityEntry>> contextChangeTrackingDetail = _dbContextChangeTrackingService.TrackRevisionDetails(_context);
            int result = await _context.SaveChangesAsync();

            if (contextChangeTrackingDetail.Count > 0)
            {
                await _dbContextChangeTrackingService.SaveAuditTrail(transactionBy, contextChangeTrackingDetail);
            }

            return result;
        }
    }
}
