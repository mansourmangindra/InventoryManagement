using Common.Constants;
using Common.DataTransferObjects._Core.ErrorLog;
using Common.DataTransferObjects.CommonSearch;
using Common.DataTransferObjects.Filter;
using Common.DataTransferObjects.Filter.CollectionPaging;
using Common.DataTransferObjects.UserRole;
using Common.Extension;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace ProjectUno.Web.Controllers.Maintenance
{
    [Route("UserAccount")]
    public class UserAccountController : Controller
    {

        private readonly HttpClient _httpClient;
        public UserAccountController(IHttpClientFactory httpClientFactory)
        {
            _httpClient = httpClientFactory.CreateClient(HttpClientConstant.InventoryApiNamedClient);
        }


        [HttpGet("")]
        public ActionResult Index(KeywordDateRangeActivePagination filter)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(new ErrorMessage(ErrorMessageConstant.InvalidInputData));
            }

            return PartialView("Views/Maintenance/UserAccount/UserAccount.cshtml", filter);
        }

        [HttpGet]
        [Route("ListUserAccount")]
        public async Task<IActionResult> SearchResult(KeywordDateRangeActivePagination userSearchFilter)
        {
            HttpResponseMessage response = await _httpClient.GetAsync($"api/Role/GetAllRolesJoinPagedList?{userSearchFilter.GetQueryString()}");

            if (response.IsSuccessStatusCode)
            {
                PagedList<UserRoleDetail> userRoleDetails = JsonConvert.DeserializeObject<PagedList<UserRoleDetail>>(await response.Content.ReadAsStringAsync());

                return PartialView("Views/Maintenance/UserAccount/_SearchResult.cshtml", userRoleDetails);
            }

            return RedirectToAction("StatusPage", "Error", await response.GetErrorMessage());
        }

    }
}
