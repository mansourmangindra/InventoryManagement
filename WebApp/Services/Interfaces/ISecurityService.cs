using Common.DataTransferObjects._Core.ReferenceData;
using Common.DataTransferObjects.UserRole;
using System.Security.Principal;

namespace WebApp.Services.Interfaces
{
    public interface ISecurityService
    {
        Task<IEnumerable<ReferenceDataDetail>> GetModules(IPrincipal principal);
        Task<IEnumerable<ReferenceDataDetail>> GetRoles(IPrincipal principal);
        Task<IEnumerable<UserRoleWithModule>> GetRolesWithModule(IPrincipal principal);
        Task<bool> WithModule(short[] moduleIds, IPrincipal principal);
        Task<bool> WithRole(short[] roleIds, IPrincipal principal);
        Task<UserRoleWithModule> GetSelectedRole(IPrincipal principal);
    }
}
