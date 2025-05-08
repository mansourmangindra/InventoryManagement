

using DataAccess.DbContexts.InventoryManagement.Models;

namespace ApiConfiguration.Service
{
    public interface IJwtTokenGenerator
    {
        string GenerateToken(User userDetail, IEnumerable<string> roles);
    }
}
