using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using WareHouseManagementSystem.Models;

namespace WareHouseManagementSystem.Controllers
{
    public class HomeController : BaseController
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
            if (IsLoggedIn())
            {
                return View();
            }
            else
            {
                return RedirectToAction("SignIn", "Account");
            }
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}