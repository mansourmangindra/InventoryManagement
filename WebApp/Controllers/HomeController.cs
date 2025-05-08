using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using WebApp.Models;

namespace WebApp.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Logout()
        {
            var redirectUrl = Url.Content("~/");
            var properties = new AuthenticationProperties { RedirectUri = redirectUrl };

            return SignOut(properties, CookieAuthenticationDefaults.AuthenticationScheme);
        }
    }
}
