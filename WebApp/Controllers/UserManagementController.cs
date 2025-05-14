using Common.Constants;
using Common.DataTransferObjects.CommonSearch;
using Common.DataTransferObjects.Filter.CollectionPaging;
using Common.DataTransferObjects.User;
using Common.Extension;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using WebApp.Models.User;
using WebApp.Services.Interfaces;

namespace WebApp.Controllers
{
    public class UserManagementController : Controller
    {
        private readonly HttpClient _httpClient;
        public UserManagementController(IHttpClientFactory httpClientFactory)
        {
            _httpClient = httpClientFactory.CreateClient(HttpClientConstant.InventoryApiNamedClient);

        }

        public IActionResult Index()
        {
            ViewData["ActiveModule"] = "UserCreate";
            ViewData["title"] = "Create User";
            ViewData["ViewMode"] = ViewModeConstant.Create;
            return View();
        }

        [Route("UserManagement/Search")]
        public IActionResult Search(UserSearchFilter userSearchFilter)
        {
            ViewData["ActiveModule"] = "UserSearch";
            ViewData["title"] = "User Search";

            UserSearchViewModel userSearchViewModel = new()
            {
                UserSearchFilter = userSearchFilter
            };

            return View("Views/User/Search.cshtml", userSearchViewModel);
        }

        [HttpGet]
        [Route("UserManagement/List")]
        public async Task<IActionResult> ListAsync(UserSearchFilter userSearchFilter)
        {
            HttpResponseMessage response = await _httpClient.GetAsync($"api/User/UserListSearch?{userSearchFilter.GetQueryString()}");

            if (response.IsSuccessStatusCode)
            {
                PagedList<UserDetail> userDetails = JsonConvert.DeserializeObject<PagedList<UserDetail>>(await response.Content.ReadAsStringAsync());

                foreach (UserDetail userDetail in userDetails.Items)
                {
                    userDetail.UserRoleDetails = userDetail.UserRoleDetails.Where(ur => ur.Active);
                }

                return PartialView("Views/User/_List.cshtml", userDetails);
            }

            return RedirectToAction("StatusPage", "Error", await response.GetErrorMessage());

        }
    }
}
