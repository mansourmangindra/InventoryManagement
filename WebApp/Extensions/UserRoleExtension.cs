
using Common.Constants;
using Common.DataTransferObjects._Core.AppSettings;
using Common.DataTransferObjects.UserRole;
using System.Security.Claims;
using System.Security.Principal;
using WebApp.Services.Interfaces;

namespace WebApp.Extensions
{
    public static class UserRoleExtension
    {
        public static List<string> ApplicationRoleName(this IPrincipal User)
        {
            //TODO: Customize Application role
            IConfiguration configuration = StaticConfiguration.Configuration;
            List<string> applicationRole = new();
            SecurityGroup securityGroup = new();
            configuration.Bind("SecurityGroup", securityGroup);

            ClaimsIdentity claimsIdentity = (ClaimsIdentity)User.Identity;
            IEnumerable<Claim> groupClaims = claimsIdentity.Claims.Where(c => c.Type == "groups").ToList();

            if (groupClaims.Any(c => securityGroup.ApplicationSupport.Contains(c.Value)))
            {
                applicationRole.Add(RoleConstant.Support);
            }

            return applicationRole;
        }

        public async static Task<int> SelectedRoleId(this IPrincipal principal, ISecurityService securityService)
        {
            UserRoleWithModule userRoleWithModule = await securityService.GetSelectedRole(principal);
            if (userRoleWithModule != null)
            {
                return int.Parse(userRoleWithModule.RoleDetail.Value.ToString());
            }
            else
            {
                return 0;
            }
        }

        public async static Task<UserRoleWithModule> SelectedRoleWithModule(this IPrincipal principal, ISecurityService securityService)
        {
            return await securityService.GetSelectedRole(principal);
        }
    }
}
