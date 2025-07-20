using Common.DataTransferObjects.UserRole;
using Microsoft.EntityFrameworkCore;

namespace DataAccess.DbContexts.InventoryManagement
{
    public partial class InventoryManagementDbContext : DbContext
    {
        public virtual DbSet<UserRoleDetail> UserRoleJoin { get; set; }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<UserRoleDetail>().HasNoKey().ToView(null);
            modelBuilder.Entity<UserRoleDetail>().Ignore(x => x.urdList);
        }
    }

}
