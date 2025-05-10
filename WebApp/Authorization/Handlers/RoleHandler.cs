using Microsoft.AspNetCore.Authorization;
using WebApp.Services.Interfaces;
using WebApp.Authorization.Requirements;

namespace WebApp.Authorization.Handlers
{
    public class RoleHandler : AuthorizationHandler<RoleRequirement>
    {
        private readonly ISecurityService _securityService;
        private readonly IHttpContextAccessor _httpContextAccessor;
        public RoleHandler(ISecurityService securityService, IHttpContextAccessor httpContextAccessor)
        {
            _securityService = securityService;
            _httpContextAccessor = httpContextAccessor;
        }

        protected override async Task HandleRequirementAsync(AuthorizationHandlerContext context, RoleRequirement requirement)
        {
            if (await requirement.Pass(_securityService, _httpContextAccessor.HttpContext.User))
                context.Succeed(requirement);
            else
                context.Fail();
        }
    }
}