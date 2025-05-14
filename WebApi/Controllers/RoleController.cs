using Common.Constants;
using Common.DataTransferObjects._Core.ErrorLog;
using Common.DataTransferObjects.CommonSearch;
using Common.DataTransferObjects.Filter.CollectionPaging;
using Common.DataTransferObjects.PositionType;
using Common.DataTransferObjects.Role;
using Common.DataTransferObjects.RoleModule;
using Common.DataTransferObjects.User;
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
    }
}
