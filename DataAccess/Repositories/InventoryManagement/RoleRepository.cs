
using Common.DataTransferObjects.CommonSearch;
using Common.DataTransferObjects.Filter;
using Common.DataTransferObjects.Filter.CollectionPaging;
using Common.DataTransferObjects.Role;
using Common.DataTransferObjects.UserRole;
using DataAccess.DbContexts.InventoryManagement;
using DataAccess.DbContexts.InventoryManagement.Models;
using DataAccess.Repositories.Base;
using DataAccess.Repositories.InventoryManagement.Interfaces;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using System.Data;

namespace DataAccess.Repositories.InventoryManagement
{
    public class RoleRepository : BaseRepository<Role>, IRoleRepository
    {
        private readonly InventoryManagementDbContext _dbContext;
        public RoleRepository(InventoryManagementDbContext context) : base(context)
        {
            _dbContext = context;

        }

        public async Task<IEnumerable<Role>> GetRoleAllFromSP()
        {
            try
            {
                return await _dbContext.Set<Role>()
                .FromSqlRaw("EXEC dbo.[GetAllRoles]")
                .ToListAsync();
            }
            catch (Exception ex)
            {
                Console.WriteLine("SP call failed: " + ex.Message);
                throw;
            }


        }

        public async Task<PagedList<Role>> GetRoleAllFromSPPagedListAsync(BasicSearchFilter filter)
        {
            var pagingParams = new PagingParameter
            {
                PageNumber = filter.PageNumber,
                PageSize = filter.PageSize
            };

            var keyword = string.IsNullOrWhiteSpace(filter.Keyword) || filter.Keyword == "undefined"
            ? (object)DBNull.Value
            : filter.Keyword;

            var parameters = new[]
            {
                new SqlParameter("@PageIndex", filter.PageNumber),
                new SqlParameter("@PageEntry", filter.PageSize),
                new SqlParameter("@Sort", filter.SortOrder ?? "asc"),  
                new SqlParameter("@SortBy", filter.SortBy),        
                new SqlParameter("@SearchBy", filter.Keyword ?? (object)DBNull.Value)
            };
            return await GetSPPagedListAsync(
                "GetAllRolesPagedList",
                parameters,
                pagingParams,
                reader => new Role
                {
                    RoleId = reader.GetInt16(reader.GetOrdinal("RoleId")),
                    Name = reader.GetString(reader.GetOrdinal("Name")),
                    CreatedBy = reader.GetString(reader.GetOrdinal("CreatedBy")),
                    UpdatedBy = reader.IsDBNull(reader.GetOrdinal("UpdatedBy")) ? null : reader.GetString(reader.GetOrdinal("UpdatedBy")),
                    CreatedDate = reader.GetDateTime(reader.GetOrdinal("CreatedDate")),
                    UpdatedDate = reader.GetDateTime(reader.GetOrdinal("UpdatedDate")),
                    Active = reader.GetBoolean(reader.GetOrdinal("Active")),
                }
            );
        }


        public async Task<IEnumerable<UserRoleDetail>> GetRoleAllFromSPJoin()
        {
            try
            {
                return await _dbContext.UserRoleJoin
                .FromSqlRaw("EXEC dbo.[RolesWithJoin]")
                .ToListAsync();
            }
            catch (Exception ex)
            {
                Console.WriteLine("SP call failed: " + ex.Message);
                throw;
            }


        }

        public async Task<PagedList<UserRoleDetail>> GetRoleAllFromSPJoinPagedList(KeywordDateRangeActivePagination filter)
        {
            var pagingParams = new PagingParameter
            {
                PageNumber = filter.PageNumber,
                PageSize = filter.PageSize
            };

            var keyword = string.IsNullOrWhiteSpace(filter.Keyword) || filter.Keyword == "undefined"
               ? (object)DBNull.Value
               : filter.Keyword;

            var parameters = new[]
            {
                    new SqlParameter("@PageIndex", filter.PageNumber),
                    new SqlParameter("@PageEntry", filter.PageSize),
                    new SqlParameter("@Sort", filter.SortOrder ?? "asc"), 
                    new SqlParameter("@SortBy", filter.SortBy),        
                    new SqlParameter("@SearchBy", keyword ?? (object)DBNull.Value),
                    new SqlParameter("@StartDate", filter.StartDate ?? (object)DBNull.Value),
                    new SqlParameter("@EndDate", filter.EndDate ?? (object)DBNull.Value)
                };

            return await GetSPPagedListAsync<UserRoleDetail>(
                "RolesWithJoinPaged",
                parameters,
                pagingParams,
                reader => new UserRoleDetail
                {
                    UserId = reader.GetInt32(reader.GetOrdinal("UserId")),
                    UserName = reader.IsDBNull(reader.GetOrdinal("UserName")) ? null : reader.GetString(reader.GetOrdinal("UserName")),
                    RoleName = reader.IsDBNull(reader.GetOrdinal("RoleName")) ? null : reader.GetString(reader.GetOrdinal("RoleName")),
                    PhoneNumber = reader.IsDBNull(reader.GetOrdinal("PhoneNumber")) ? null : reader.GetString(reader.GetOrdinal("PhoneNumber")),
                    EmailAddress = reader.IsDBNull(reader.GetOrdinal("EmailAddress")) ? null : reader.GetString(reader.GetOrdinal("EmailAddress")),
                    Password = reader.IsDBNull(reader.GetOrdinal("Password")) ? null : reader.GetString(reader.GetOrdinal("Password")),
                    CreatedBy = reader.IsDBNull(reader.GetOrdinal("CreatedBy")) ? null : reader.GetString(reader.GetOrdinal("CreatedBy")),
                    UpdatedBY = reader.IsDBNull(reader.GetOrdinal("UpdatedBy")) ? null : reader.GetString(reader.GetOrdinal("UpdatedBy")),
                    CreatedDate = reader.GetDateTime(reader.GetOrdinal("CreatedDate")),
                    UpdatedDate = reader.GetDateTime(reader.GetOrdinal("UpdatedDate")),
                    Active = reader.GetBoolean(reader.GetOrdinal("Active"))
                    
                }
            );
        }

        public async Task<Role> CreateRoleAsync(int roleId, string name, string createdBy, string updatedBy, bool active = true)
        {
            var parameters = new[]
            {
                new SqlParameter("@RoleId", roleId),
            new SqlParameter("@Name", name),
            new SqlParameter("@CreatedBy", createdBy),
            new SqlParameter("@UpdatedBy", updatedBy),
            new SqlParameter("@Active", active)
        };

            return await AddAsyncSP("dbo.CreateRole", parameters, reader =>
            {
                return new Role
                {
                    RoleId = reader.GetInt16(reader.GetOrdinal("RoleId")),
                    Name = reader.GetString(reader.GetOrdinal("Name")),
                    CreatedBy = reader.GetString(reader.GetOrdinal("CreatedBy")),
                    UpdatedBy = reader.GetString(reader.GetOrdinal("UpdatedBy")),
                    CreatedDate = reader.GetDateTime(reader.GetOrdinal("CreatedDate")),
                    UpdatedDate = reader.GetDateTime(reader.GetOrdinal("UpdatedDate")),
                    Active = reader.GetBoolean(reader.GetOrdinal("Active"))
                };
            });
        }

        public async Task<Role> UpdateRoleAsync(int roleId, string name, string updatedBy, bool active)
        {
            var parameters = new[]
            {
                new SqlParameter("@RoleId", roleId),
                new SqlParameter("@Name", name),
                new SqlParameter("@UpdatedBy", updatedBy),
                new SqlParameter("@Active", active)
            };

            return await EditAsyncSP("dbo.UpdateRole", parameters, reader =>
            {
                return new Role
                {
                    RoleId = reader.GetInt16(reader.GetOrdinal("RoleId")),
                    Name = reader.GetString(reader.GetOrdinal("Name")),
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