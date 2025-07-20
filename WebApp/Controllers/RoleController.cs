using Common.Constants;
using Common.DataTransferObjects._Core.ErrorLog;
using Common.DataTransferObjects.CommonSearch;
using Common.DataTransferObjects.Filter;
using Common.DataTransferObjects.Filter.CollectionPaging;
using Common.DataTransferObjects.Role;
using Common.Extension;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace WebApp.Controllers
{
    public class RoleController : Controller
    {
        private readonly HttpClient _httpClient;
        public RoleController(IHttpClientFactory httpClientFactory)
        {
            _httpClient = httpClientFactory.CreateClient(HttpClientConstant.InventoryApiNamedClient);
        }

        [HttpGet]
        public ActionResult Index(BasicSearchFilter filter)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(new ErrorMessage(ErrorMessageConstant.InvalidInputData));
            }

            return View("Views/Role/Index.cshtml", filter);
        }

        [HttpGet]
        [Route("Role/ListRole")]
        public async Task<IActionResult> SearchResult(BasicSearchFilter roleSearchFilter)
        {
            HttpResponseMessage response = await _httpClient.GetAsync($"api/Role/GetAllRolesPagedList?{roleSearchFilter.GetQueryString()}");

            if (response.IsSuccessStatusCode)
            {
                PagedList<RoleDetail> roleDetails = JsonConvert.DeserializeObject<PagedList<RoleDetail>>(await response.Content.ReadAsStringAsync());

                return PartialView("Views/User/_RoleList.cshtml", roleDetails);
            }

            return RedirectToAction("StatusPage", "Error", await response.GetErrorMessage());
        }
    }
}
