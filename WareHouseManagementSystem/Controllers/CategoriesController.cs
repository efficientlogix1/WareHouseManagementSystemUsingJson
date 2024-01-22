using Microsoft.AspNetCore.Hosting.Server;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Reflection;
using System.Text.Json;
using WareHouseManagementSystem.Models;

namespace WareHouseManagementSystem.Controllers
{
    public class CategoriesController : BaseController
    {
        private readonly ILogger<CategoriesController> _logger;
        private Microsoft.AspNetCore.Hosting.IHostingEnvironment Environment;

        public CategoriesController(ILogger<CategoriesController> logger, Microsoft.AspNetCore.Hosting.IHostingEnvironment environment)
        {
            _logger = logger;
            Environment = environment;

        }

        public IActionResult Index()   //It show's register view.
        {
            if (IsLoggedIn())  //if logged in then... 
            {
                List<Categories> model = new List<Categories>();   //Create's user list.
                var _sampleJsonFilePath = this.Environment.WebRootPath + "\\JsonFiles\\categories.json";   //Set path from json file.
                using StreamReader reader = new(_sampleJsonFilePath); //Create reader variable read data from user.json file.
                var json = reader.ReadToEnd(); //Read data.
                if (!string.IsNullOrEmpty(json))    // check if file is not empty.
                {
                    var jarray = JArray.Parse(json);   //Create array from json.
                    foreach (var item in jarray)    // Read data from array.
                    {
                        Categories category = item.ToObject<Categories>();  //type casting.
                        model.Add(category);  // add category object to list
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
        public IActionResult Create() //shows create view
        {
            if (IsLoggedIn()) // 
            {
                return View();  // return view
            }
            else
            {
                return RedirectToAction("SignIn", "Account");  // redirect to SignIn
            }
        }
        [HttpPost]
        public IActionResult Create(Categories model)  // get category object from view
        {
            List<Categories> categories = new List<Categories>(); //Create user list
            var _sampleJsonFilePath = this.Environment.WebRootPath + "\\JsonFiles\\categories.json"; //Set path from json file
            using StreamReader reader = new(_sampleJsonFilePath); //Create reader variable read data from user.json file.
            var json = reader.ReadToEnd(); //Read data.
            if (!string.IsNullOrEmpty(json))// check if file is not empty.
            {
                var jarray = JArray.Parse(json);     //Create array from json.
                foreach (var item in jarray)      // Read data from array.
                {
                    Categories category = item.ToObject<Categories>(); //type casting.
                    categories.Add(category); // add category object to list.
                }
                if (categories.Count > 0)  // check exsisting category list.
                {
                    model.Id = categories.OrderByDescending(x => x.Id).FirstOrDefault().Id + 1; // set id next id +.
                }
            }
            reader.Close(); //close reader
            if (model.Id == 0) // check model object id 
            {
                model.Id = 1; //add 1 in model object id
            }
            categories.Add(model);
            string jsonData = JsonConvert.SerializeObject(categories, Formatting.None);  // creating json string.
            System.IO.File.WriteAllText(_sampleJsonFilePath, jsonData); //  write json string to.
            return RedirectToAction("index");  // Redirect to Index
        }
        public IActionResult Edit(int id) // shows edit view
        {
            if (IsLoggedIn())// if logged in
            {
                List<Categories> categories = new List<Categories>();  //Create user list
                var _sampleJsonFilePath = this.Environment.WebRootPath + "\\JsonFiles\\categories.json";//Set path from json file
                using StreamReader reader = new(_sampleJsonFilePath);//Create reader variable read data from user.json file.
                var json = reader.ReadToEnd(); //Read data.
                Categories model = new Categories(); //create categories object
                if (!string.IsNullOrEmpty(json)) // check if file is not empty.
                {
                    var jarray = JArray.Parse(json);    //Create array from json.
                    foreach (var item in jarray)     // Read data from array.
                    {
                        Categories category = item.ToObject<Categories>(); //type casting.
                        if (category.Id == id)
                        {
                            model = category;
                            break;
                        }
                    }
                }
                reader.Close(); // clsoe reader
                return View(model); // return view
            }
            else
            {
                return RedirectToAction("SignIn", "Account"); //Redirect to signin
            }

        }
        [HttpPost]
        public IActionResult Edit(Categories model) // get category object from view
        {
            List<Categories> categories = new List<Categories>();   //Create user list
            var _sampleJsonFilePath = this.Environment.WebRootPath + "\\JsonFiles\\categories.json"; //Set path from json file
            using StreamReader reader = new(_sampleJsonFilePath);//Create reader variable read data from user.json file.
            var json = reader.ReadToEnd();//Read data.

            if (!string.IsNullOrEmpty(json)) // check if file is not empty.
            {
                var jarray = JArray.Parse(json);    //Create array from json.
                foreach (var item in jarray)     // Read data from array.
                {
                    Categories category = item.ToObject<Categories>(); //type casting
                    if (category.Id == model.Id)
                    {
                        category.Name = model.Name;
                    }
                    categories.Add(category);
                }
            }
            reader.Close();// close reader
            string jsonData = JsonConvert.SerializeObject(categories, Formatting.None);// creating json string.
            System.IO.File.WriteAllText(_sampleJsonFilePath, jsonData); //  write json string to.
            return RedirectToAction("index"); // redirect to index
        }
        public IActionResult Details(int id) // get category object from view
        {
            if (IsLoggedIn())
            {
                List<Categories> categories = new List<Categories>(); //Create user list
                var _sampleJsonFilePath = this.Environment.WebRootPath + "\\JsonFiles\\categories.json";//Set path from json file
                using StreamReader reader = new(_sampleJsonFilePath);//Create reader variable read data from user.json file.
                var json = reader.ReadToEnd();//Read data.
                CategoryDetailViewModel model = new CategoryDetailViewModel();//create categories object
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
                if (!string.IsNullOrEmpty(json)) // check if file is not empty.
                {
                    var jarray = JArray.Parse(json);   //Create array from json.
                    foreach (var item in jarray)    // Read data from array.
                    {
                        Categories category = item.ToObject<Categories>();//type casting
                        if (category.Id == id)
                        {
                            model.Id = category.Id;
                            model.Name = category.Name;
                            model.Products = new List<Product>();
                            model.Products = products.Where(x => x.Category == model.Id).ToList();
                            break;
                        }
                    }
                }
                reader.Close(); //close reader
                return View(model);
            }
            else
            {
                return RedirectToAction("SignIn", "Account"); //Redirect to sigin 
            }

        }
        public IActionResult Delete(int id) // get category object from view
        {
            if (IsLoggedIn())
            {
                List<Categories> categories = new List<Categories>();//Create user list
                var _sampleJsonFilePath = this.Environment.WebRootPath + "\\JsonFiles\\categories.json";//Set path from json file
                using StreamReader reader = new(_sampleJsonFilePath);//Create reader variable read data from user.json file.
                var json = reader.ReadToEnd();//Read data.
                List<Product> products = new List<Product>();//Create user list
                var _sampleJsonFilePathProduct = this.Environment.WebRootPath + "\\JsonFiles\\products.json";//Set path from json file
                using StreamReader readerp = new(_sampleJsonFilePathProduct);//Create reader variable read data from user.json file.
                var jsonp = readerp.ReadToEnd();//Read data.
                CategoryDetailViewModel model = new CategoryDetailViewModel(); //create categories object
                if (!string.IsNullOrEmpty(jsonp)) // check if file is not empty.
                {
                    var jarray = JArray.Parse(jsonp);  //Create array from json.
                    foreach (var item in jarray)     // Read data from array.
                    {
                        Product product = item.ToObject<Product>(); //type casting
                        products.Add(product);// add product object to list 
                    }
                }
                readerp.Close(); //close reader
                if (!string.IsNullOrEmpty(json)) // check if file is not empty.
                {
                    var jarray = JArray.Parse(json);  //Create array from json.
                    foreach (var item in jarray)    // Read data from array.
                    {
                        Categories category = item.ToObject<Categories>();//type casting
                        if (category.Id == id)
                        {
                            model.Id = category.Id;
                            model.Name = category.Name;
                            model.Products = new List<Product>();
                            List<Product> productList = new List<Product>();//Create user list
                            productList = products.Where(p => p.Category == id).ToList();
                            if (productList.Count > 0) // check model object id 
                            {
                                model.Products = products.Where(x => x.Category == model.Id).ToList();
                                return View(model); // return view
                            }
                        }
                    }
                }
                reader.Close(); // close reader
                return View(model);
            }
            else
            {
                return RedirectToAction("SignIn", "Account"); // redirect to signin 
            }

        }
        [HttpPost]
        [HttpPost, ActionName("Delete")]
        public IActionResult DeleteConfirmed(int id)// get category object from view
        {
            List<Categories> categories = new List<Categories>();//Create user list
            var _sampleJsonFilePath = this.Environment.WebRootPath + "\\JsonFiles\\categories.json";//Set path from json file
            using StreamReader reader = new(_sampleJsonFilePath);//Create reader variable read data from user.json file.
            var json = reader.ReadToEnd();//Read data.
            List<Product> products = new List<Product>();//Create user list
            var _sampleJsonFilePathProduct = this.Environment.WebRootPath + "\\JsonFiles\\products.json";//Set path from json file
            using StreamReader readerp = new(_sampleJsonFilePathProduct);//Create reader variable read data from user.json file.
            var jsonp = readerp.ReadToEnd();//Read data.
            CategoryDetailViewModel model = new CategoryDetailViewModel();//create categories object
            if (!string.IsNullOrEmpty(jsonp)) // check if file is not empty.
            {
                var jarray = JArray.Parse(jsonp);  //Create array from json.
                foreach (var item in jarray)     // Read data from array.
                {
                    Product product = item.ToObject<Product>();//type casting
                    products.Add(product);// add product object to list 
                }
            }
            readerp.Close(); // close readerp

            if (!string.IsNullOrEmpty(json))// check if file is not empty.
            {
                var jarray = JArray.Parse(json); //Create array from json.
                foreach (var item in jarray)    // Read data from array.
                {
                    Categories category = item.ToObject<Categories>(); //type casting

                    if (category.Id == id)
                    {
                        List<Product> productList = new List<Product>();//Create user list
                        productList = products.Where(p => p.Category == id).ToList();
                        if (productList.Count > 0) // check model object id 
                        {
                            ViewBag.ErrMsg = "this category is already in use"; // shows error msg
                            model.Id = category.Id;
                            model.Name = category.Name;
                            model.Products = new List<Product>();
                            model.Products = products.Where(x => x.Category == model.Id).ToList();
                            return View(model);
                        }
                    }
                    else
                    {
                        categories.Add(category);
                    }
                }
            }
            reader.Close(); // close reader
            string jsonData = JsonConvert.SerializeObject(categories, Formatting.None);// creating json 
            System.IO.File.WriteAllText(_sampleJsonFilePath, jsonData);//  write json string to.
            return RedirectToAction("index"); //redirect to index
        }
    }
}