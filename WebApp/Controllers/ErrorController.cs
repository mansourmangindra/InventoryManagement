using Common.Constants;
using Common.DataTransferObjects._Core.BasicFilter;
using Common.DataTransferObjects._Core.CollectionPaging;
using Common.DataTransferObjects._Core.ErrorLog;
using Common.DataTransferObjects._Core.Version;
using Common.Extension;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Primitives;
using Newtonsoft.Json;
using WebApp.Models.Error;

namespace WebApp.Controllers
{
    [Route("[controller]")]
    public class ErrorController : Controller
    {
        private readonly HttpClient _httpClient;
        public ErrorController(IHttpClientFactory httpClientFactory)
        {
            _httpClient = httpClientFactory.CreateClient(HttpClientConstant.InventoryApiNamedClient);
        }

        [HttpGet]
        public ActionResult Index(KeywordDateRangePagination filter)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(new ErrorMessage(ErrorMessageConstant.InvalidInputData));
            }

            return View("Views/Error/Index.cshtml", filter);
        }

        [HttpGet]
        [Route("_searchresult")]
        public async Task<IActionResult> SearchResult(KeywordDateRangePagination filter)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(new ErrorMessage(ErrorMessageConstant.InvalidInputData));
            }

            HttpResponseMessage response = await _httpClient.GetAsync($"api/ErrorLog?{filter.GetQueryString()}");
            if (response.IsSuccessStatusCode)
            {
                PagedList<ErrorLogDetail> pgErrorLogDetail = JsonConvert.DeserializeObject<PagedList<ErrorLogDetail>>(await response.Content.ReadAsStringAsync());

                return PartialView("Views/Error/_SearchResult.cshtml", pgErrorLogDetail);
            }
            return BadRequest(await response.GetErrorMessage());
        }

        [HttpGet]
        [Route("Detail/{id}")]
        public async Task<IActionResult> Detail([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(new ErrorMessage(ErrorMessageConstant.InvalidInputData));
            }

            var response = await _httpClient.GetAsync($"api/ErrorLog/{id}");
            if (response.IsSuccessStatusCode)
            {
                ErrorLogDetail errorLogDetail = JsonConvert.DeserializeObject<ErrorLogDetail>(await response.Content.ReadAsStringAsync());
                return View("Views/Error/Detail.cshtml", errorLogDetail);
            }

            return RedirectToAction("StatusPage", "Error", await response.GetErrorMessage());
        }

        [AllowAnonymous]
        public IActionResult AccessDenied()
        {
            return RedirectToAction("StatusPage", "Error", new { code = 403 });
        }

        [AllowAnonymous]
        [Route("StatusPage")]
        public IActionResult StatusPage(ErrorMessage errorMessage, int code)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(new ErrorMessage(ErrorMessageConstant.InvalidInputData));
            }

            StatusPageViewModel model;

            if (string.IsNullOrEmpty(errorMessage.Type) && code != 0)
            {
                model = new(code, User.Identity.Name);
            }
            else
            {
                model = new(errorMessage);
            }

            return View("Views/Error/StatusPage.cshtml", model);
        }

        [AllowAnonymous]
        [Route("LogError")]
        public async Task<IActionResult> LogError()
        {
            string user = User.Identity.Name;

            IExceptionHandlerPathFeature context = HttpContext.Features.Get<IExceptionHandlerPathFeature>();

            SaveErrorLog saveErrorLog = new()
            {
                Message = context.Error.InnerException != null
                    ? $"{context.Error.Message} | {context.Error.InnerException.Message}"
                    : context.Error.Message,

                StackTraceId = HttpContext.TraceIdentifier,
                StackTrace = context.Error.StackTrace,
                Path = context.Path,
                DateCreated = DateTime.Now,
                UserIdentity = user ?? "UnAuthenticated User",
                Source = context.Error.Source,
                BuildVersion = VersionDetail.DisplayVersion()
            };

            await _httpClient.PostAsync($"api/ErrorLog", saveErrorLog.GetStringContent());
            Request.Headers.TryGetValue("x-requested-with", out StringValues requestedWith);

            ErrorMessage errorMessage;

            if (requestedWith.Equals("XMLHttpRequest"))
            {
                if (context.Error.GetType() == typeof(ArgumentException))
                {
                    errorMessage = new(saveErrorLog.StackTraceId, ErrorMessageTypeConstant.ArgumentException, saveErrorLog.Message);
                }
                else
                {
                    errorMessage = new(saveErrorLog.StackTraceId, ErrorMessageTypeConstant.InternalServerException, null);
                }

                return BadRequest(errorMessage);
            }

            if (context.Error.GetType() == typeof(ArgumentException))
            {
                errorMessage = new(saveErrorLog.StackTraceId, ErrorMessageTypeConstant.ArgumentException, saveErrorLog.Message);
            }
            else
            {
                errorMessage = new(saveErrorLog.StackTraceId, ErrorMessageTypeConstant.InternalServerException, null);
            }

            return RedirectToAction("StatusPage", "Error", errorMessage);
        }
    }
}