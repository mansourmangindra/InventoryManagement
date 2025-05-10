using Common.DataTransferObjects._Core.ReferenceData;
using Common.DataTransferObjects.UserRole;
using DataAccess.UnitOfWorks.InventoryManagement;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserRoleController : ControllerBase
    {
        private readonly IInventoryUnitOfWork _inventoryUnitOfWork;
        public UserRoleController(IInventoryUnitOfWork inventoryUnitOfWork)
        {
            _inventoryUnitOfWork = inventoryUnitOfWork;
        }

        [HttpGet("UserPrincipalName/{userPrincipalName}/UserRoleWithModuleDetails")]
        [SwaggerOperation(Summary = "Get User Roles with Modules by UserPrincipalName")]
        public async Task<ActionResult<IEnumerable<UserRoleWithModule>>> GetUserRoleWithModulesByUpn([FromRoute] string userPrincipalName)
        {
            IEnumerable<UserRoleWithModule> userRoleWithModuleDetails = await _inventoryUnitOfWork.UserRoleRepository.FindAsync(
               selector: ur => new UserRoleWithModule()
               {
                   RoleDetail = new ReferenceDataDetail()
                   {
                       Value = ur.Role.RoleId,
                       Name = ur.Role.Name,
                       Active = ur.Role.Active
                   },

                   ModuleDetails = ur.Role.RoleModules
                    .Where(rm => rm.Active)
                    .Select(rm => new ReferenceDataDetail()
                    {
                        Value = rm.Module.ModuleId,
                        Name = rm.Module.Name,
                        Active = rm.Module.Active
                    }),
               },
               predicate: ur => ur.User.EmailAddress == userPrincipalName &&
                ur.Active &&
                ur.User.Active &&
                ur.Role.Active,
               orderBy: o => o.OrderBy(ur => ur.Role.Name));

            return Ok(userRoleWithModuleDetails);
        }

    }
}
