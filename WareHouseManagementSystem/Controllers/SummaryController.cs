using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Linq;
using WareHouseManagementSystem.Models;

namespace WareHouseManagementSystem.Controllers
{
    public class SummaryController : BaseController
    {
        private readonly ILogger<SummaryController> _logger;
        private Microsoft.AspNetCore.Hosting.IHostingEnvironment Environment;

        public SummaryController(ILogger<SummaryController> logger, Microsoft.AspNetCore.Hosting.IHostingEnvironment environment)
        {
            _logger = logger;
            Environment = environment;

        }
        public IActionResult CategorySummary()
        {
            if (IsLoggedIn())  //if logged in then... 
            {
                List<CategoryDetailViewModel> model = new List<CategoryDetailViewModel>();   //Create Categories list.
                var _sampleJsonFilePath = this.Environment.WebRootPath + "\\JsonFiles\\categories.json";   //Set path from json file.
                using StreamReader reader = new(_sampleJsonFilePath); //Create reader variable read data from user.json file.
                List<Product> products = new List<Product>();//Create user list
                var _sampleJsonFilePathProduct = this.Environment.WebRootPath + "\\JsonFiles\\products.json"; //Set path from json file
                using StreamReader readerp = new(_sampleJsonFilePathProduct);//Create reader variable read data from user.json file.
                var jsonp = readerp.ReadToEnd();//Read data.
                if (!string.IsNullOrEmpty(jsonp)) // check if file is not empty.
                {
                    var jarray = JArray.Parse(jsonp);    //Create array from json.
                    foreach (var item in jarray)      // Read data from array.
                    {
                        Product product = item.ToObject<Product>(); //type casting
                        products.Add(product);
                    }
                }
                readerp.Close();
                var json = reader.ReadToEnd(); //Read data.
                if (!string.IsNullOrEmpty(json))    // check if file is not empty.
                {
                    var jarray = JArray.Parse(json);   //Create array from json.
                    foreach (var item in jarray)    // Read data from array.
                    {
                        CategoryDetailViewModel categoryDetailViewModel = new CategoryDetailViewModel();//create categories object
                        Categories category = item.ToObject<Categories>();  //type casting.
                        categoryDetailViewModel.Name = category.Name;
                        categoryDetailViewModel.Id = category.Id;
                        categoryDetailViewModel.Products = new List<Product>();
                        categoryDetailViewModel.Products = products.Where(x => x.Category == categoryDetailViewModel.Id).ToList();
                        model.Add(categoryDetailViewModel);  // add category object to list
                    }
                }
                model = model.OrderBy(x => x.Id).ToList();  // sorting list to order by id 
                return View(model);  // return value
            }
            else
            {
                return RedirectToAction("SignIn", "Account");  // redirect to SignIn
            }
        }
        public IActionResult ProductSummary()
        {
            if (IsLoggedIn())//if logged in then... 
            {
                List<Product> model = new List<Product>();// Create's user list.
                var _sampleJsonFilePath = this.Environment.WebRootPath + "\\JsonFiles\\products.json"; //Set path from json file.
                using StreamReader reader = new(_sampleJsonFilePath);//Create reader variable read data from user.json file.                     
                var json = reader.ReadToEnd();//Read data.
                if (!string.IsNullOrEmpty(json))// check if file is not empty.
                {
                    var jarray = JArray.Parse(json);  //Create array from json.
                    foreach (var item in jarray) // Read data from array.
                    {
                        Product product = item.ToObject<Product>();  //type casting.
                        model.Add(product);  // add category object to list       
                    }
                }
                model = model.OrderBy(x => x.Id).ToList();// sorting list to order by id   
                return View(model); // return value
            }
            else
            {
                return RedirectToAction("SignIn", "Account");// redirect to SignIn
            }
        }
    }
}
