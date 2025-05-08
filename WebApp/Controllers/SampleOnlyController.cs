using Common.Constants;
using Common.DataTransferObjects._Core.ErrorLog;
using Common.DataTransferObjects.Notification;
using Common.Extension;
using Microsoft.AspNetCore.Mvc;

namespace WebApp.Controllers
{
    [Route("[controller]")]
    public class SampleOnlyController : Controller
    {
        private readonly HttpClient _httpClient;
        public SampleOnlyController(IHttpClientFactory httpClientFactory)
        {
            _httpClient = httpClientFactory.CreateClient(HttpClientConstant.InventoryApiNamedClient);
        }


        [HttpGet]
        [Route("Layout/Horizontal")]
        public IActionResult HorizontalLayout()
        {
            return View("Views/SampleOnly/HorizontalLayout.cshtml");
        }

        [HttpGet]
        [Route("Layout/Vertical")]
        public IActionResult VerticalLayout()
        {
            return View("Views/SampleOnly/VerticalLayout.cshtml");
        }

        [HttpGet]
        [Route("Demo")]
        public IActionResult Demo()
        {
            return View("Views/SampleOnly/Demo.cshtml");
        }

        
    }
}
