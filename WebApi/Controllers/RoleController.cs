using Common.Constants;
using Common.DataTransferObjects._Core.ErrorLog;
using Common.DataTransferObjects.CommonSearch;
using Common.DataTransferObjects.Filter;
using Common.DataTransferObjects.Filter.CollectionPaging;
using Common.DataTransferObjects.PositionType;
using Common.DataTransferObjects.Role;
using Common.DataTransferObjects.RoleModule;
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
    public class RoleController : ControllerBase
    {
        private readonly IInventoryUnitOfWork _inventoryUnitOfWork;
        public RoleController(IInventoryUnitOfWork inventoryUnitOfWork)
        {
            _inventoryUnitOfWork = inventoryUnitOfWork;
        }

        [HttpGet]
        [Route("RoleListSearch")]
        [SwaggerOperation(Summary = "Search Role with Paging")]
        public async Task<ActionResult<PagedList<PositionTypeDetail>>> Search([FromQuery] BasicSearchFilter basicSearchFilter)
        {
            PagedList<RoleDetail> roleDetails = await _inventoryUnitOfWork.RoleRepository.GetPagedListAsync(
                selector: r => new RoleDetail()
                {
                    RoleId = r.RoleId,
                    Name = r.Name,
                    UpdatedBy = r.UpdatedBy,
                    UpdatedDate = r.UpdatedDate,
                    Active = r.Active
                },
                predicate: r => (String.IsNullOrEmpty(basicSearchFilter.Keyword)
                                || r.Name.Contains(basicSearchFilter.Keyword))
                                && (basicSearchFilter.Active == null || !basicSearchFilter.Active.Any() || basicSearchFilter.Active.Contains(r.Active)),
                pagingParameter: basicSearchFilter,
                orderBy: o => o.OrderByDescending(r => r.CreatedDate));


            return Ok(roleDetails);
        }

        [HttpGet("{id}")]
        [SwaggerOperation(Summary = "Get Role Details by ID")]
        public async Task<ActionResult<RoleDetail>> Get(int id)
        {
            RoleDetail roleDetail = await _inventoryUnitOfWork.RoleRepository.SingleOrDefaultAsync(
                selector: r => new RoleDetail()
                {
                    RoleId = id,
                    Name = r.Name,
                    UpdatedBy = r.UpdatedBy,
                    UpdatedDate = r.UpdatedDate,
                    Active = r.Active,
                    RoleModuleDetails = r.RoleModules
                    .Where(rm => rm.Active)
                    .Select(rm => new RoleModuleDetail()
                    {
                        RoleId = rm.RoleId,
                        ModuleId = rm.ModuleId,
                        Active = rm.Active
                    }),
                    ListOfUsers = r.UserRoles
                    .Where(ur => ur.Active)
                    .Select(ur => new UserDetail()
                    {
                        Name = ur.User.Name,
                        PhoneNumber = ur.User.PhoneNumber,
                        EmailAddress = ur.User.EmailAddress
                    })
                },
                predicate: r => r.RoleId == id);

            if (roleDetail == null)
            {
                return NotFound(new ErrorMessage(ErrorMessageTypeConstant.NotFound, ErrorMessageConstant.IdNotFound("Role", id.ToString())));
            }

            return Ok(roleDetail);
        }

        [HttpGet]
        [Route("GetAllRoles")]
        [SwaggerOperation(Summary = "Get All Roles via Stored Procedure")]
        public async Task<ActionResult<IEnumerable<RoleDetail>>> GetAllUserDetailsFromSP()
        {
            IEnumerable<Role> accountDetails = await _inventoryUnitOfWork.RoleRepository.GetRoleAllFromSP();

            if (accountDetails == null || !accountDetails.Any())
            {
                return NotFound(new ErrorMessage(ErrorMessageTypeConstant.NotFound));
            }

            return Ok(accountDetails);
        }

        [HttpGet]
        [Route("GetAllRolesPagedList")]
        [SwaggerOperation(Summary = "Get All Roles PagedList via Stored Procedure")]
        public async Task<ActionResult<PagedList<Role>>> GetAllRoleDetailsPagedFromSP([FromQuery] BasicSearchFilter filter)
        {
            PagedList<Role> accountDetails = await _inventoryUnitOfWork.RoleRepository.GetRoleAllFromSPPagedListAsync(filter);

            return Ok(accountDetails);
        }


        [HttpGet]
        [Route("GetAllRolesJoin")]
        [SwaggerOperation(Summary = "Get All Roles Join via Stored Procedure")]
        public async Task<ActionResult<IEnumerable<UserRoleDetail>>> GetAllUserDetailsFromSPJoin()
        {
            IEnumerable<UserRoleDetail> accountDetails = await _inventoryUnitOfWork.RoleRepository.GetRoleAllFromSPJoin();

            if (accountDetails == null || !accountDetails.Any())
            {
                return NotFound(new ErrorMessage(ErrorMessageTypeConstant.NotFound));
            }

            return Ok(accountDetails);
        }

        [HttpGet]
        [Route("GetAllRolesJoinPagedList")]
        [SwaggerOperation(Summary = "Get All Roles Join PagedList via Stored Procedure")]
        public async Task<ActionResult<PagedList<UserRoleDetail>>> GetAllRoleDetailsJoinPagedFromSP([FromQuery] KeywordDateRangeActivePagination filter)
        {
            PagedList<UserRoleDetail> accountDetails = await _inventoryUnitOfWork.RoleRepository.GetRoleAllFromSPJoinPagedList(filter);

            return Ok(accountDetails);

        }

        [HttpPost]
        [Route("TransactionBy/{transactionBy}")]
        [SwaggerOperation(Summary = "Create Role")]
        public async Task<ActionResult<int>> Create([FromRoute] string transactionBy, [FromBody] RoleDto roleDetail)
        {

            await _inventoryUnitOfWork.RoleRepository.CreateRoleAsync(
                roleDetail.RoleId,
                roleDetail.Name,
                transactionBy,
                transactionBy,
                roleDetail.Active
            );

            await _inventoryUnitOfWork.SaveChangesAsync(transactionBy);

            return Ok(roleDetail.RoleId);
        }


        [HttpPost("{roleId}/TransactionBy/{transactionBy}")]
        [SwaggerOperation(Summary = "Update Role by ID")]
        public async Task<ActionResult> Update([FromRoute] int roleId, [FromRoute] string transactionBy, [FromBody] UpdateRoleDto roleDetail)
        {
            await _inventoryUnitOfWork.RoleRepository.UpdateRoleAsync(
               roleId,
               roleDetail.Name,
               transactionBy,
               false
           );

            Role newRole = await _inventoryUnitOfWork.RoleRepository.CreateRoleAsync(
                roleDetail.RoleId,
                roleDetail.Name,
                transactionBy,
                transactionBy,
                true
             );



            await _inventoryUnitOfWork.SaveChangesAsync(transactionBy);

            return Ok();
        }



    }
}
