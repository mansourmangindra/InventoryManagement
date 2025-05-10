using Microsoft.AspNetCore.Authorization;
using WebApp.Services.Interfaces;
using WebApp.Authorization.Requirements;

namespace WebApp.Authorization.Handlers
{
    public class ModuleHandler : AuthorizationHandler<ModuleRequirement>
    {
        private readonly ISecurityService _securityService;
        private readonly IHttpContextAccessor _httpContextAccessor;
        public ModuleHandler(ISecurityService securityService, IHttpContextAccessor httpContextAccessor)
        {
            _securityService = securityService;
            _httpContextAccessor = httpContextAccessor;
        }

        protected override async Task HandleRequirementAsync(AuthorizationHandlerContext context, ModuleRequirement requirement)
        {
            if (await requirement.Pass(_securityService, _httpContextAccessor.HttpContext.User))
                context.Succeed(requirement);
            else
                context.Fail();
        }
    }
}