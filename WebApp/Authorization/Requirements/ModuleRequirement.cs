using Microsoft.AspNetCore.Authorization;
using System.Security.Principal;
using WebApp.Services.Interfaces;

namespace WebApp.Authorization.Requirements
{
    public class ModuleRequirement : IAuthorizationRequirement
    {
        private readonly short[] _moduleIds;
        public ModuleRequirement(short[] moduleIds)
        {
            _moduleIds = moduleIds;
        }

        public async Task<bool> Pass(ISecurityService securityService, IPrincipal principal)
        {
            if (await securityService.WithModule(_moduleIds, principal))
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