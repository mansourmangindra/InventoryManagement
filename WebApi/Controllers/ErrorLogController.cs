using Common.Constants;
using Common.DataTransferObjects._Core.BasicFilter;
using Common.DataTransferObjects._Core.CollectionPaging;
using Common.DataTransferObjects._Core.ErrorLog;
using DataAccess.DbContexts.InventoryManagement.Models;
using DataAccess.UnitOfWorks.InventoryManagement;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    //[Authorize(Policy = PolicyNameConstant.AllData)]
    public class ErrorLogController : ControllerBase
    {
        private readonly IInventoryUnitOfWork _inventoryUnitOfWork;
        public ErrorLogController(IInventoryUnitOfWork inventoryUnityOfWork)
        {
            _inventoryUnitOfWork = inventoryUnityOfWork;
        }

        [HttpPost]
        [SwaggerOperation(Summary = "Log Application Error")]
        public async Task<IActionResult> Create(SaveErrorLog saveErrorLog)
        {
            ErrorLog errorLog = new()
            {
                BuildVersion = saveErrorLog.BuildVersion,
                DateCreated = DateTime.Now,
                Message = saveErrorLog.Message,
                Path = saveErrorLog.Path,
                Source = saveErrorLog.Source,
                StackTrace = saveErrorLog.StackTrace,
                StackTraceId = saveErrorLog.StackTraceId,
                UserIdentity = saveErrorLog.UserIdentity
            };

            await _inventoryUnitOfWork.ErrorLogRepository.AddAsync(errorLog);
            await _inventoryUnitOfWork.SaveChangesAsync(saveErrorLog.UserIdentity);

            return Ok();
        }

        [HttpGet("{id}")]
        [SwaggerOperation(Summary = "Get Error Details by Id")]
        public async Task<ActionResult<ErrorLogDetail>> Get(int id)
        {
            ErrorLogDetail errorLogDetail = await _inventoryUnitOfWork.ErrorLogRepository.
                FirstOrDefaultAsync(selector: e => new ErrorLogDetail()
                {
                    BuildVersion = e.BuildVersion,
                    DateCreated = e.DateCreated,
                    ErrorId = e.ErrorId,
                    Message = e.Message,
                    Path = e.Path,
                    Source = e.Source,
                    StackTrace = e.StackTrace,
                    StackTraceId = e.StackTraceId,
                    UserIdentity = e.UserIdentity
                },
                predicate: e => e.ErrorId == id);

            if (errorLogDetail == null)
            {
                return NotFound(new ErrorMessage(ErrorMessageTypeConstant.NotFound, $"Error ID not exist: {id}"));
            }
            else
            {
                return Ok(errorLogDetail);
            }
        }

        [HttpGet]
        [SwaggerOperation(Summary = "Search Error Log with Paging")]
        public async Task<ActionResult<PagedList<ErrorLogDetail>>> Search([FromQuery] KeywordDateRangePagination filter)
        {
            PagedList<ErrorLogDetail> errorLogDetails = await _inventoryUnitOfWork.ErrorLogRepository.GetPagedListAsync(
                selector: e => new ErrorLogDetail()
                {
                    ErrorId = e.ErrorId,
                    BuildVersion = e.BuildVersion,
                    DateCreated = e.DateCreated,
                    Message = e.Message,
                    Path = e.Path,
                    Source = e.Source,
                    StackTrace = e.StackTrace,
                    StackTraceId = e.StackTraceId,
                    UserIdentity = e.UserIdentity
                },
                predicate: e => (filter.StartDate == null || e.DateCreated >= filter.StartDate) &&
                                 (filter.EndDate == null || e.DateCreated <= filter.EndDate) &&
                                 (string.IsNullOrEmpty(filter.Keyword) || e.BuildVersion.Contains(filter.Keyword) ||
                                                                          e.Message.Contains(filter.Keyword) ||
                                                                          e.Path.Contains(filter.Keyword) ||
                                                                          e.Source.Contains(filter.Keyword) ||
                                                                          e.StackTrace.Contains(filter.Keyword) ||
                                                                          e.StackTraceId.Contains(filter.Keyword) ||
                                                                          e.UserIdentity.Contains(filter.Keyword)),
                pagingParameter: filter,
                orderBy: o => o.OrderByDescending(e => e.DateCreated));

            return Ok(errorLogDetails);
        }
    }
}