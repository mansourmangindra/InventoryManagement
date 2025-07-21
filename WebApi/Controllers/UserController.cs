using Azure;
using Common.Constants;
using Common.DataTransferObjects._Core.ErrorLog;
using Common.DataTransferObjects.CommonSearch;
using Common.DataTransferObjects.Filter.CollectionPaging;
using Common.DataTransferObjects.Role;
using Common.DataTransferObjects.User;
using Common.DataTransferObjects.UserRole;
using DataAccess.DbContexts.InventoryManagement.Models;
using DataAccess.UnitOfWorks.InventoryManagement;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using WebApi.Services.Registration;

namespace WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IInventoryUnitOfWork _inventoryUnitOfWork;
        private readonly IAuthService _authService;
        public UserController(IInventoryUnitOfWork inventoryUnitOfWork, IAuthService authService)
        {
            _inventoryUnitOfWork = inventoryUnitOfWork;
            _authService = authService;
        }

        //Create User[ADMIN] or Register User
        [HttpPost]
        [Route("TransactionBy/{transactionBy}")]
        [SwaggerOperation(Summary = "Create User")]
        public async Task<ActionResult<int>> CreateUser([FromRoute] string transactionBy, [FromBody] SaveUser saveUserDto)
        {
            bool existing = await _inventoryUnitOfWork.UserRepository.IsExistAsync(
                predicate: u => u.EmailAddress != null && u.EmailAddress == saveUserDto.EmailAddress);

            if (existing)
            {
                return BadRequest(new ErrorMessage(ErrorMessageConstant.AlreadyExist($"{saveUserDto.EmailAddress}")));
            }


            string userId = await _authService.Register(transactionBy, saveUserDto);
            return Ok(userId);


        }

        //Put Admin Only 
        [HttpPut("{id}/TransactionBy/{transactionBy}")]
        [SwaggerOperation(Summary = "Update User by ID")]
        public async Task<ActionResult> Update([FromRoute] int id, [FromRoute] string transactionBy, [FromBody] SaveUser saveUser)
        {
            bool existing = await _inventoryUnitOfWork.UserRepository.IsExistAsync(
                predicate: u => u.EmailAddress == saveUser.EmailAddress && u.UserId != id);

            if (existing)
            {
                return BadRequest(new ErrorMessage(ErrorMessageConstant.AlreadyExist("Name")));
            }

            User user = await _inventoryUnitOfWork.UserRepository.GetAsync(id);

            if (user == null)
            {
                return NotFound(new ErrorMessage(ErrorMessageTypeConstant.NotFound, ErrorMessageConstant.IdNotFound("User", id.ToString())));
            }


            user.EmailAddress = saveUser.EmailAddress;
            user.PhoneNumber = saveUser.PhoneNumber;
            user.Name = saveUser.Name;
            user.UpdatedBy = transactionBy;
            user.UpdatedDate = DateTime.UtcNow;
            user.Active = saveUser.Active;

            await _inventoryUnitOfWork.SaveChangesAsync(transactionBy);

            return Ok();
        }

        [HttpDelete("DeleteUser/{id}/TransactionBy/{transactionBy}")]
        [SwaggerOperation(Summary = "Hard Delete User by ID")]
        public async Task<ActionResult> Delete([FromRoute] int id, [FromRoute] string transactionBy)
        {
            User user = await _inventoryUnitOfWork.UserRepository.GetAsync(id);

            if (user == null)
            {
                return NotFound(new ErrorMessage(ErrorMessageTypeConstant.NotFound, ErrorMessageConstant.IdNotFound("User", id.ToString())));
            }

            _inventoryUnitOfWork.UserRepository.Remove(user);
            await _inventoryUnitOfWork.SaveChangesAsync(transactionBy);

            return Ok();
        }



        [HttpGet]
        [Route("GetUserDetailsByUPN/{upn}")]
        [SwaggerOperation(Summary = "Get User Details by UPN")]
        public async Task<ActionResult<UserDetail>> Get([FromRoute] string upn)
        {
            UserDetail userDetail = await _inventoryUnitOfWork.UserRepository.SingleOrDefaultAsync(
                selector: u => new UserDetail()
                {
                    UserId = u.UserId,
                    EmailAddress = u.EmailAddress,
                    PhoneNumber = u.PhoneNumber,
                    CreatedBy = u.CreatedBy,
                    CreatedDate = u.CreatedDate,
                    UpdatedBy = u.UpdatedBy,
                    UpdatedDate = u.UpdatedDate,
                    Active = u.Active
                },
                predicate: a => a.EmailAddress.ToLower() == upn.ToLower());


            if (userDetail == null)
            {
                return NotFound(new ErrorMessage(ErrorMessageTypeConstant.NotFound, ErrorMessageConstant.EntityNotFound(upn)));
            }


            return Ok(userDetail);
        }

        [HttpGet("{id}")]
        [SwaggerOperation(Summary = "Get User Details by ID")]
        public async Task<ActionResult<UserDetail>> Get(int id)
        {
            UserDetail userDetail = await _inventoryUnitOfWork.UserRepository.SingleOrDefaultAsync(
                selector: u => new UserDetail()
                {
                    UserId = u.UserId,
                    EmailAddress = u.EmailAddress,
                    PhoneNumber = u.PhoneNumber,
                    CreatedBy = u.CreatedBy,
                    CreatedDate = u.CreatedDate,
                    UpdatedBy = u.UpdatedBy,
                    UpdatedDate = u.UpdatedDate,
                    Active = u.Active
                },
                predicate: a => a.UserId == id);

            if (userDetail == null)
            {
                return NotFound(new ErrorMessage(ErrorMessageTypeConstant.NotFound, ErrorMessageConstant.IdNotFound("User", id.ToString())));
            }

            return Ok(userDetail);
        }

        [HttpGet]
        [Route("UserListSearch")]
        [SwaggerOperation(Summary = "Search User with Paging")]
        public async Task<ActionResult<PagedList<UserDetail>>> Search([FromQuery] UserSearchFilter userSearchFilter)
        {
            PagedList<UserDetail> userDetails = await _inventoryUnitOfWork.UserRepository.GetPagedListAsync(
                selector: u => new UserDetail()
                {
                    UserId = u.UserId,
                    Name = u.Name,
                    PhoneNumber = u.PhoneNumber,
                    EmailAddress = u.EmailAddress,
                    UserRoleDetails = u.UserRoles.Select(ur => new UserRoleDetail()
                    {
                        RoleId = ur.Role.RoleId,
                        RoleName = ur.Role.Name,
                        Active = ur.Active
                    }),
                    UpdatedBy = u.UpdatedBy,
                    UpdatedDate = u.UpdatedDate,
                    Active = u.Active
                },
                predicate: u => (String.IsNullOrEmpty(userSearchFilter.SearchKeyword) || u.EmailAddress.Contains(userSearchFilter.SearchKeyword)
                                                                                      || u.Name.Contains(userSearchFilter.SearchKeyword)) &&
                    (userSearchFilter.Active == null || !userSearchFilter.Active.Any() || userSearchFilter.Active.Contains(u.Active)) &&
                    (userSearchFilter.RoleIds == null || !userSearchFilter.RoleIds.Any() || u.UserRoles.Any(w => userSearchFilter.RoleIds.Contains(w.RoleId))),
                pagingParameter: userSearchFilter,
                orderBy: o => o.OrderBy(u => u.EmailAddress).ThenBy(u => u.CreatedDate));

            return Ok(userDetails);
        }


        [HttpPost]
        [Route("user/{user}")]
        [SwaggerOperation(Summary = "Create or Update User")]
        public async Task<ActionResult<int>> CreateOrUpdateUser([FromRoute] string user, [FromBody] UserDto userDetail)
        {
            int action;
            if (userDetail.UserId == 0)
                action = 1;
            else if (!userDetail.Active)
                action = 3;
            else
                action = 2;

            var result = await _inventoryUnitOfWork.UserRepository.CreateOrUpdateUserAsync(
                action,
                userDetail.UserId,
                userDetail.Name,
                userDetail.PhoneNumber,
                userDetail.EmailAddress,
                userDetail.Password,
                user,
                user,
                userDetail.Active
            );

            await _inventoryUnitOfWork.SaveChangesAsync(user);

            return Ok(result.UserId);
        }




    }
}
