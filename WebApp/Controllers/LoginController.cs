using Common.Constants;
using Common.DataTransferObjects._Core.ErrorLog;
using Common.DataTransferObjects.User;
using Common.Extension;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace WebApp.Controllers
{
    public class LoginController : Controller
    {
        private readonly HttpClient _httpClient;
        public LoginController(IHttpClientFactory httpClientFactory)
        {
            _httpClient = httpClientFactory.CreateClient(HttpClientConstant.InventoryApiNamedClient);
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login([FromBody] LoginRequest loginRequest)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(new ErrorMessage(ErrorMessageConstant.InvalidInputData));
            }

            var response = await _httpClient.PostAsync($"api/Login/TransactionBy/{loginRequest.EmailAddress}", loginRequest.GetStringContent());
            if (response.IsSuccessStatusCode) 
            {
                return Json(new { redirectUrl = Url.Action("Index", "Home") });
            }

            return BadRequest(await response.GetErrorMessage());



        }

    }
}
