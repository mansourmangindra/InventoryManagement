using Common.Constants;
using Common.DataTransferObjects._Core.ReferenceData;
using DataAccess.UnitOfWorks.InventoryManagement;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ReferenceDataController : ControllerBase
    {
        private readonly IInventoryUnitOfWork _inventoryUnitOfWork;
        public ReferenceDataController(IInventoryUnitOfWork inventoryUnitOfWork)
        {
            _inventoryUnitOfWork = inventoryUnitOfWork;
        }

        [HttpGet]
        [Route("Roles")]
        [SwaggerOperation(Summary = "Get Roles")]
        public async Task<ActionResult<IEnumerable<ReferenceDataDetail>>> GetRoles([FromQuery] IEnumerable<bool> actives)
        {
            IEnumerable<ReferenceDataDetail> referenceDataDetails = await _inventoryUnitOfWork.RoleRepository.FindAsync(
                selector: r => new ReferenceDataDetail()
                {
                    Name = r.Name,
                    Value = r.RoleId,
                    Active = r.Active
                },
                predicate: r => (r.Active),
                orderBy: r => r.OrderBy(o => o.Name));
            return Ok(referenceDataDetails);
        }

        
    }
}
