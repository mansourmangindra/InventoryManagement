using Microsoft.AspNetCore.Mvc;

namespace ProjectUno.Web.Controllers
{
	public class MaintenanceController : Controller
	{
        private readonly ILogger<MaintenanceController> _logger;

        public MaintenanceController(ILogger<MaintenanceController> logger)
        {
            _logger = logger;
        }
        public IActionResult Index()
		{
			return View();
		}

        public IActionResult HRCertificate()
        {
            return PartialView("HRCertificate");
        }
        public IActionResult CardInventory()
        {
            return PartialView("CardInventory");
        }
        public IActionResult AccountVariant()
        {
            return PartialView("AccountVariant");
        }
        public IActionResult CompanyClassification()
        {
            return PartialView("CompanyClassification");
        }
        public IActionResult PayrollAccountOpeningMode()
        {
            return PartialView("PayrollAccountOpeningMode");
        }
        public IActionResult CDDStatus()
        {
            return PartialView("CDDStatus");
        }
        public IActionResult Branch()
        {
            return PartialView("Branch");
        }
        public IActionResult UserAccount()
        {
            return PartialView("UserAccount");
        }
        public IActionResult UserRole()
        {
            return PartialView("UserRole");
        }

        public IActionResult TermsandConditions()
        {
            return PartialView("TermsandConditions");
        }
    }
}
