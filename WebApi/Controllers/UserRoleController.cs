using Common.Constants;
using Common.DataTransferObjects._Core.ErrorLog;
using Common.DataTransferObjects._Core.ReferenceData;
using Common.DataTransferObjects.User;
using Common.DataTransferObjects.UserRole;
using DataAccess.DbContexts.InventoryManagement.Models;
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


        [HttpGet("{id}")]
        [SwaggerOperation(Summary = "Get UserRole Details by ID")]
        public async Task<ActionResult<UserRoleDetail>> Get(int id)
        {
            UserRoleDetail userRoleDetail = await _inventoryUnitOfWork.UserRoleRepository.SingleOrDefaultAsync(
                selector: u => new UserRoleDetail()
                {
                    UserId = u.UserId,
                    UserRoleId = u.UserRoleId,
                    RoleId = u.Role.RoleId,
                    UpdatedBy = u.UpdatedBy,
                    UpdatedDate = u.UpdatedDate
                },
                predicate: u => u.UserRoleId == id);

            if (userRoleDetail == null)
            {
                return NotFound(new ErrorMessage(ErrorMessageTypeConstant.NotFound, ErrorMessageConstant.IdNotFound("User Role", id.ToString())));
            }

            return Ok(userRoleDetail);
        }

        //Adding Role using ADMIN ONLY
        [HttpPost]
        [Route("TransactionBy/{transactionBy}")]
        [SwaggerOperation(Summary = "Create UserRole")]
        public async Task<ActionResult<int>> Create([FromRoute] string transactionBy, [FromBody] SaveUserRole saveUserRole)
        {
            bool existing = await _inventoryUnitOfWork.UserRoleRepository.IsExistAsync(
                predicate: u => u.UserId == saveUserRole.UserId && u.RoleId == saveUserRole.RoleId && u.Active);

            if (existing)
            {
                return BadRequest(new ErrorMessage(ErrorMessageConstant.AlreadyExist("User Role")));
            }

            UserRole userRole = new()
            {
                UserId = saveUserRole.UserId,
                RoleId = saveUserRole.RoleId,
                Active = saveUserRole.Active,
                CreatedBy = transactionBy,
                UpdatedBy = transactionBy,
                CreatedDate = DateTime.UtcNow,
                UpdatedDate = DateTime.UtcNow
            };

            await _inventoryUnitOfWork.UserRoleRepository.AddAsync(userRole);

            await _inventoryUnitOfWork.SaveChangesAsync(transactionBy);

            return Ok(userRole.UserRoleId);
        }

        //Edit using ADMIN only
        [HttpPut("{id}/TransactionBy/{transactionBy}")]
        [SwaggerOperation(Summary = "Update UserRole by ID")]
        public async Task<ActionResult> Update([FromRoute] int id, [FromRoute] string transactionBy, [FromBody] SaveUserRole saveUserRole)
        {
            bool existing = await _inventoryUnitOfWork.UserRoleRepository.IsExistAsync(
                predicate: u => u.UserId == saveUserRole.UserId && u.RoleId == saveUserRole.RoleId && u.UserRoleId != id && u.Active);

            if (existing)
            {
                return BadRequest(new ErrorMessage(ErrorMessageConstant.AlreadyExist("User Role")));
            }


            UserRole userRole = await _inventoryUnitOfWork.UserRoleRepository.GetAsync(id);

            if (userRole == null)
            {
                return NotFound(new ErrorMessage(ErrorMessageTypeConstant.NotFound, ErrorMessageConstant.IdNotFound("UserRole", id.ToString())));
            }


            userRole.RoleId = saveUserRole.RoleId;
            userRole.UpdatedBy = transactionBy;
            userRole.UpdatedDate = DateTime.UtcNow;
            userRole.Active = saveUserRole.Active;

            await _inventoryUnitOfWork.SaveChangesAsync(transactionBy);

            return Ok();
        }

        [HttpDelete("{id}/TransactionBy/{transactionBy}")]
        [SwaggerOperation(Summary = "Hard Delete UserRole by ID")]
        public async Task<ActionResult<UserDetail>> Delete([FromRoute] int id, [FromRoute] string transactionBy)
        {
            UserRole userRole = await _inventoryUnitOfWork.UserRoleRepository.GetAsync(id);

            if (userRole == null)
            {
                return NotFound(new ErrorMessage(ErrorMessageTypeConstant.NotFound, ErrorMessageConstant.IdNotFound("UserRole", id.ToString())));
            }


            UserDetail userDetail = await _inventoryUnitOfWork.UserRepository.SingleOrDefaultAsync(
                selector: u => new UserDetail()
                {
                    EmailAddress = u.EmailAddress
                },
                predicate: u => u.UserId == userRole.UserId && u.Active);

            _inventoryUnitOfWork.UserRoleRepository.Remove(userRole);

            await _inventoryUnitOfWork.SaveChangesAsync(transactionBy);

            return Ok(userDetail);
        }

    }
}
