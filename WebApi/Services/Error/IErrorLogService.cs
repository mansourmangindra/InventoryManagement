using Common.DataTransferObjects._Core.ErrorLog;

namespace WebAPI.Services.Error
{
    public interface IErrorLogService
    {
        Task<ErrorMessage> LogApiError(HttpContext context);
    }
}