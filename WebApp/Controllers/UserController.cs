using Common.Constants;
using Common.DataTransferObjects.User;
using Common.Extension;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using WebApp.Services.Interfaces;

namespace WebApp.Controllers
{
    public class UserController : Controller
    {
        private readonly HttpClient _httpClient;
        private readonly ITokenProvider _tokenProvider;
        public UserController(IHttpClientFactory httpClientFactory, ITokenProvider tokenProvider)
        {
            _httpClient = httpClientFactory.CreateClient(HttpClientConstant.InventoryApiNamedClient);
            _tokenProvider = tokenProvider;
        }

        public ActionResult Register()
        {
            ViewData["ActiveModule"] = "RegisterView";

           
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Register([FromBody] SaveUser saveUserDto)
        {
            var response = await _httpClient.PostAsync($"api/User/TransactionBy/{saveUserDto.EmailAddress}", saveUserDto.GetStringContent());

            if (response.IsSuccessStatusCode)
            {
                int id = JsonConvert.DeserializeObject<int>(await response.Content.ReadAsStringAsync());
                return Ok(id);
            }
            else
            {
                return BadRequest(await response.GetErrorMessage());
            }
        }

        
    }
}
