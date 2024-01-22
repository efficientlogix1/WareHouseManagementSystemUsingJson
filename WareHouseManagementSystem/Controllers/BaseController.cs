using Microsoft.AspNetCore.Mvc;

namespace WareHouseManagementSystem.Controllers
{
    public class BaseController : Controller
    {
        public bool IsLoggedIn()
        {
            bool loggedIn = false;
            if (!string.IsNullOrEmpty(HttpContext.Session.GetString("username"))) //check session value.
            {
                loggedIn = true; // logged in true
                ViewBag.Username = HttpContext.Session.GetString("username");  // set viewbag variable for current user login
                ViewBag.Password = HttpContext.Session.GetString("password"); //set viewbag variable for curren password login
            }
            return loggedIn;
        }
    }
}
