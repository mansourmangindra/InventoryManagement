
using DataAccess.DbContexts.InventoryManagement;
using DataAccess.DbContexts.InventoryManagement.Models;
using DataAccess.Repositories.Base;
using DataAccess.Repositories.InventoryManagement.Interfaces;
using Microsoft.Data.SqlClient;

namespace DataAccess.Repositories.InventoryManagement
{
    public class UserRepository : BaseRepository<User>, IUserRepository
    {
        private readonly InventoryManagementDbContext _dbContext;
        public UserRepository(InventoryManagementDbContext context) : base(context)
        {
            _dbContext = context;
        }

        public async Task<User> CreateOrUpdateUserAsync(int action, int? userId, string name, string phoneNumber, string emailAddress, string password, string createdBy, string updatedBy, bool active)
        {
            var parameters = new[]
            {
                new SqlParameter("@Action", action),
                new SqlParameter("@UserId", userId ?? (object)DBNull.Value),
                new SqlParameter("@Name", name ?? (object)DBNull.Value),
                new SqlParameter("@PhoneNumber", phoneNumber ?? (object)DBNull.Value),
                new SqlParameter("@EmailAddress", emailAddress ?? (object)DBNull.Value),
                new SqlParameter("@Password", password ?? (object)DBNull.Value),
                new SqlParameter("@CreatedBy", createdBy ?? (object)DBNull.Value),
                new SqlParameter("@UpdatedBy", updatedBy ?? (object)DBNull.Value),
                new SqlParameter("@Active", active)
            };

            return await EditAsyncSP("dbo.sp_UserAccountAddEdit", parameters, reader =>
            {
                return new User
                {
                    UserId = reader.GetInt32(reader.GetOrdinal("UserId")),
                    Name = reader.GetString(reader.GetOrdinal("Name")),
                    PhoneNumber = reader.GetString(reader.GetOrdinal("PhoneNumber")),
                    EmailAddress = reader.GetString(reader.GetOrdinal("EmailAddress")),
                    Password = reader.GetString(reader.GetOrdinal("Password")),
                    CreatedBy = reader.GetString(reader.GetOrdinal("CreatedBy")),
                    UpdatedBy = reader.GetString(reader.GetOrdinal("UpdatedBy")),
                    CreatedDate = reader.GetDateTime(reader.GetOrdinal("CreatedDate")),
                    UpdatedDate = reader.GetDateTime(reader.GetOrdinal("UpdatedDate")),
                    Active = reader.GetBoolean(reader.GetOrdinal("Active"))
                };
            });
        }

    }
}