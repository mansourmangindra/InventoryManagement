using Microsoft.AspNetCore.Authorization;
using System.Security.Principal;
using WebApp.Services.Interfaces;

namespace WebApp.Authorization.Requirements
{
    public class RoleRequirement : IAuthorizationRequirement
    {
        private readonly short[] _roleIds;
        public RoleRequirement(short[] roleIds)
        {
            _roleIds = roleIds;
        }

        public async Task<bool> Pass(ISecurityService securityService, IPrincipal principal)
        {
            if (await securityService.WithRole(_roleIds, principal))
            {
                return await Task.FromResult(true);
            }
            else
            {
                return await Task.FromResult(false);
            }
        }
    }
}