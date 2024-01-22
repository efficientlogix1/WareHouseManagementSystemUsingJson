using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using WareHouseManagementSystem.Models;

namespace WareHouseManagementSystem.Controllers
{
    public class AccountController : BaseController
    {
        private readonly ILogger<AccountController> _logger;
        private Microsoft.AspNetCore.Hosting.IHostingEnvironment Environment;

        public AccountController(ILogger<AccountController> logger, Microsoft.AspNetCore.Hosting.IHostingEnvironment environment)
        {
            _logger = logger;
            Environment = environment;

        }
        
        public IActionResult Register()  //It show's register view.
        {
            return View();
        }

        [HttpPost]
        public IActionResult Register(User model)
        {
            List<User> users = new List<User>();  //Create's user list.
            var _sampleJsonFilePath = this.Environment.WebRootPath + "\\JsonFiles\\users.json";  //Set path from json file.
            using StreamReader reader = new(_sampleJsonFilePath);  //Create reader variable read data from user.json file.
            var json = reader.ReadToEnd();  //Read data.
            if (!string.IsNullOrEmpty(json))  // check if file is not empty.
            {
                var jarray = JArray.Parse(json);  //Create array from json.
                foreach (var item in jarray)     // Read data from array.
                {
                    User user = item.ToObject<User>();  //type casting.
                    if (user.Username == model.Username)  // check if same user name already exsist.
                    {
                        ViewBag.ErrMsg = "this username is already exists"; // set error msg.
                        return View(model); // return model view.
                    }
                    users.Add(user); // add exsisting user object to user list.
                }
                users.Add(model);  //  add new user object to user list.
            }
            reader.Close(); // close reader

            string jsonData = JsonConvert.SerializeObject(users, Formatting.None);  // create json string.
            System.IO.File.WriteAllText(_sampleJsonFilePath, jsonData); // write data to file.
            HttpContext.Session.SetString("username", model.Username);  // set username.
            HttpContext.Session.SetString("password", model.Password);  // set password.
            return RedirectToAction("index","Home");  // Redirect to Index page.
        }

        public IActionResult SignIn()  // it shows SignIn view.
        {
            return View();
        }
        [HttpPost]
        public IActionResult SignIn(User model)
        {
            var _sampleJsonFilePath = this.Environment.WebRootPath + "\\JsonFiles\\users.json";  //Set path from json file.
            using StreamReader reader = new(_sampleJsonFilePath);  //Create reader variable read data from user.json file.
            var json = reader.ReadToEnd();   //Read data.
            if (!string.IsNullOrEmpty(json))  // check if file is not empty.
            {
                var jarray = JArray.Parse(json);  //Create array from json.
                foreach (var item in jarray)     // Read data from array.
                {
                    User user = item.ToObject<User>();//type casting.
                    if (user.Username.ToLower() == model.Username.ToLower() && user.Password == model.Password)  // check if same user name already exsist.
                    {  
                        HttpContext.Session.SetString("username", model.Username); //set username in session variable.
                        HttpContext.Session.SetString("password", model.Password);  //set password in session variable.
                        return RedirectToAction("index", "Home"); //Redirect to Index page.
                    }
                }                
            }
            reader.Close();  // close reader.
            ViewBag.ErrMsg = "username or password is incorrect";  //shows error msg.
            return View(model);  // pass user object as model to view.
        }

        public IActionResult Profile()   // it shows Profile view.
        {
            if (IsLoggedIn())  // check the user is logged in
            {
                User model = new User();
                model.Username = ViewBag.Username;
                model.Password = ViewBag.Password;
                return View(model); // return view
            }
            else
            {
                return RedirectToAction("SignIn", "Account");  // if not logged in then redirect to sign in page
            }
        }
        [HttpPost]
        public IActionResult Profile(User model)  // it shows DeleteConfirmed view.
        {
            User currentUser = new User();  // create user object for currentuser.
            currentUser.Username = HttpContext.Session.GetString("username"); // get currently logged in user name.
            currentUser.Password = HttpContext.Session.GetString("password"); //get currently logged in user password.
            List<User> users = new List<User>(); //Create's user list.
            var _sampleJsonFilePath = this.Environment.WebRootPath + "\\JsonFiles\\users.json";   //Set path from json file.
            using StreamReader reader = new(_sampleJsonFilePath); //Create reader variable read data from user.json file.
            var json = reader.ReadToEnd();    //Read data.

            if (!string.IsNullOrEmpty(json)) // check if file is not empty.
            {
                var jarray = JArray.Parse(json);    //Create array from json.
                foreach (var item in jarray)       // Read data from array.
                {
                    User user = item.ToObject<User>();  //type casting.
                    if (user.Username.ToLower() == currentUser.Username.ToLower())  // check if same user name already exsist.
                    {
                        user.Username = model.Username;
                        user.Password = model.Password;
                    }
                    users.Add(user);// add user object to user list.
                }
            }
            reader.Close(); // close reader.
            string jsonData = JsonConvert.SerializeObject(users, Formatting.None);  // creating json string.
            System.IO.File.WriteAllText(_sampleJsonFilePath, jsonData);  //  write json string to.
            return RedirectToAction("index","home");  //Redirect to SignIn.
        }

        public IActionResult DeleteConfirmed()  // it shows DeleteConfirmed view.
        {
            User currentUser = new User();  // create user object for currentuser.
            currentUser.Username = HttpContext.Session.GetString("username"); // get currently logged in user name.
            currentUser.Password = HttpContext.Session.GetString("password"); //get currently logged in user password.
            List<User> users = new List<User>(); //Create's user list.
            var _sampleJsonFilePath = this.Environment.WebRootPath + "\\JsonFiles\\users.json";   //Set path from json file.
            using StreamReader reader = new(_sampleJsonFilePath); //Create reader variable read data from user.json file.
            var json = reader.ReadToEnd();    //Read data.

            if (!string.IsNullOrEmpty(json)) // check if file is not empty.
            {
                var jarray = JArray.Parse(json);    //Create array from json.
                foreach (var item in jarray)       // Read data from array.
                {
                    User user = item.ToObject<User>();  //type casting.
                    if (user.Username != currentUser.Username)  // check if same user name already exsist.
                    {
                        users.Add(user);  // add exsisting user object to user list.
                    }
                }
            }
            reader.Close(); // close reader.
            string jsonData = JsonConvert.SerializeObject(users, Formatting.None);  // creating json string.
            System.IO.File.WriteAllText(_sampleJsonFilePath, jsonData);  //  write json string to.
            return RedirectToAction("SignIn");  //Redirect to SignIn.
        }
        public IActionResult Logout()
        {
            //Delete the Session object.
            HttpContext.Session.Remove("username");
            HttpContext.Session.Remove("password");

            return RedirectToAction("SignIn");
        }
    }
}
