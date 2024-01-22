
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using WareHouseManagementSystem.Models;

namespace WareHouseManagementSystem.Controllers
{
    public class ProductController : BaseController
    {
        private readonly ILogger<ProductController> _logger;
        private Microsoft.AspNetCore.Hosting.IHostingEnvironment Environment;

        public ProductController(ILogger<ProductController> logger, Microsoft.AspNetCore.Hosting.IHostingEnvironment environment)
        {
            _logger = logger;
            Environment = environment;

        }

        public IActionResult Index()//It show's Index view.
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
        public IActionResult Create() //It show's Create view.
        {
            if (IsLoggedIn())//if logged in then...     
            {
                List<Categories> categories = new List<Categories>(); //Create's user list.
                var _sampleJsonFilePath = this.Environment.WebRootPath + "\\JsonFiles\\categories.json"; //Set path from json file.
                using StreamReader reader = new(_sampleJsonFilePath);//Create reader variable read data from user.json file.
                var json = reader.ReadToEnd();//Read data.
                if (!string.IsNullOrEmpty(json))// check if file is not empty.
                {
                    var jarray = JArray.Parse(json);//Create array from json.    
                    foreach (var item in jarray)  // Read data from array.
                    {
                        Categories category = item.ToObject<Categories>(); //type casting.
                        categories.Add(category); // add category object to list    
                    }
                }
                categories = categories.OrderBy(x => x.Id).ToList(); // sorting list to order by id 

                ViewBag.list = categories;
                return View(); // return value
            }
            else
            {
                return RedirectToAction("SignIn", "Account");// redirect to SignIn
            }
            
        }

        [HttpPost]
        public IActionResult Create(Product model)//It show's view.
        {
            List<Product> products = new List<Product>();//Create's user list.
            var _sampleJsonFilePath = this.Environment.WebRootPath + "\\JsonFiles\\products.json"; //Set path from json file.
            using StreamReader reader = new(_sampleJsonFilePath); //Create reader variable read data from user.json file.
            var json = reader.ReadToEnd(); //Read data.
            if (!string.IsNullOrEmpty(json)) // check if file is not empty.
            {
                var jarray = JArray.Parse(json); //Create array from json.
                foreach (var item in jarray) // Read data from array.
                {
                    Product product = item.ToObject<Product>(); //type casting.
                    if (product.Code.ToLower() == model.Code.ToLower())
                    {
                        ViewBag.ErrMsg = "this code is already exists"; // set error msg.
                        List<Categories> categories = new List<Categories>(); //Create's user list.
                        var _sampleJsonFilePathc = this.Environment.WebRootPath + "\\JsonFiles\\categories.json"; //Set path from json file.
                        using StreamReader readerc = new(_sampleJsonFilePathc);//Create reader variable read data from user.json file.
                        var jsonc = readerc.ReadToEnd();//Read data.
                        if (!string.IsNullOrEmpty(jsonc))// check if file is not empty.
                        {
                            var jarrayc = JArray.Parse(jsonc);//Create array from json.    
                            foreach (var itemc in jarrayc)  // Read data from array.
                            {
                                Categories category = itemc.ToObject<Categories>(); //type casting.
                                categories.Add(category); // add category object to list    
                            }
                        }
                        categories = categories.OrderBy(x => x.Id).ToList(); // sorting list to order by id 

                        ViewBag.list = categories;
                        return View(model);
                    }
                    products.Add(product);// add product object to list 
                }
                if (products.Count > 0) 
                {
                    model.Id = products.OrderByDescending(x => x.Id).FirstOrDefault().Id + 1;
                }
            }
            reader.Close();
            if (model.Id == 0)// check model object id
            {
                model.Id = 1;// add model id by 1
            }
            model.CreatedOn = DateTime.Now;// set value of Created timestamp
            products.Add(model);
            string jsonData = JsonConvert.SerializeObject(products, Formatting.None);// creating json 
            System.IO.File.WriteAllText(_sampleJsonFilePath, jsonData);//  write json string to.
            return RedirectToAction("index"); // redirect to index
        }
        public IActionResult Edit(int id)  //It show's view.
        {
            if (IsLoggedIn())//if logged in then...
            {
                var _sampleJsonFilePath = this.Environment.WebRootPath + "\\JsonFiles\\products.json"; //Set path from json file.
                using StreamReader reader = new(_sampleJsonFilePath); //Create reader variable read data from user.json file.
                var json = reader.ReadToEnd();  //Read data.
                Product model = new Product(); // create project model.
                if (!string.IsNullOrEmpty(json))// check if file is not empty.
                {
                    var jarray = JArray.Parse(json);
                    foreach (var item in jarray)
                    {
                        Product product = item.ToObject<Product>();
                        if (product.Id == id)
                        {
                            model = product;
                            break;
                        }
                    }
                }
                reader.Close();
                List<Categories> categories = new List<Categories>();//Create's user list.
                var _sampleJsonFilePath1 = this.Environment.WebRootPath + "\\JsonFiles\\categories.json";
                using StreamReader reader1 = new(_sampleJsonFilePath1); //Create reader variable read data from user.json file.
                var json1 = reader1.ReadToEnd(); //Read data.
                if (!string.IsNullOrEmpty(json1))// check if file is not empty.
                {
                    var jarray = JArray.Parse(json1);  //Create array from json.
                    foreach (var item in jarray)// Read data from array.
                    {
                        Categories category = item.ToObject<Categories>();
                        categories.Add(category);
                    }
                }
                categories = categories.OrderBy(x => x.Id).ToList();//type casting.

                ViewBag.list = categories;// create a view bag list for category dropdown list
                return View(model);//return view
            }
            else
            {
                return RedirectToAction("SignIn", "Account"); //Redirect to signin
            }
            
        }
        [HttpPost]
        public IActionResult Edit(Product model)//It show's view.
        {
            List<Product> products = new List<Product>();//Create's user list.
            var _sampleJsonFilePath = this.Environment.WebRootPath + "\\JsonFiles\\products.json"; //Set path from json file.
            using StreamReader reader = new(_sampleJsonFilePath); //Create reader variable read data from user.json file.
            var json = reader.ReadToEnd();//Read data.

            if (!string.IsNullOrEmpty(json))// check if file is not empty.
            {
                var jarray = JArray.Parse(json);//Create array from json.
                foreach (var item in jarray)// Read data from array.
                {
                    Product product = item.ToObject<Product>();//Create's user list.
                    if (product.Code.ToLower() == model.Code.ToLower() && product.Id != model.Id)//check code for product that is not open in edit
                    {
                        ViewBag.ErrMsg = "this code is already exists"; // set error msg.
                        return View(model);
                    }
                    if (product.Id == model.Id)// select the product to modify properties
                    {
                        product.Code = model.Code; // set value of code
                        product.Name = model.Name;// set value of name
                        product.Description = model.Description;// set value of description
                        product.Category = model.Category;// set value of category
                        product.Quantity = model.Quantity;// set value of quantity
                        product.Measure = model.Measure;// set value of measure
                        product.Price = model.Price;// set value of price
                        product.EditOn = DateTime.Now;// set value of edit timestamp
                    }
                    products.Add(product);

                }
            }
            reader.Close();
            string jsonData = JsonConvert.SerializeObject(products, Formatting.None);// creating json 
            System.IO.File.WriteAllText(_sampleJsonFilePath, jsonData);
            return RedirectToAction("index");
        }
        public IActionResult Details(int id)  //It show's view.
        {
            if (IsLoggedIn())
            {
                var _sampleJsonFilePath = this.Environment.WebRootPath + "\\JsonFiles\\products.json"; //Set path from json file.
                using StreamReader reader = new(_sampleJsonFilePath); //Create reader variable read data from user.json file.
                var json = reader.ReadToEnd();//Read data.
                Product model = new Product();
                if (!string.IsNullOrEmpty(json))// check if file is not empty.
                {
                    var jarray = JArray.Parse(json);//Create array from json.
                    foreach (var item in jarray)    // Read data from array.
                    {
                        Product product = item.ToObject<Product>();//Create's user list.
                        if (product.Id == id)
                        {
                            model = product;
                            break;
                        }
                    }
                }
                reader.Close();
                List<Categories> categories = new List<Categories>();//Create's user list.
                var _sampleJsonFilePath1 = this.Environment.WebRootPath + "\\JsonFiles\\categories.json"; //Set path from json file.
                using StreamReader reader1 = new(_sampleJsonFilePath1); //Create reader variable read data from user.json file.
                var json1 = reader1.ReadToEnd(); //Read data.
                if (!string.IsNullOrEmpty(json1))// check if file is not empty.
                {
                    var jarray = JArray.Parse(json1);//Create array from json.
                    foreach (var item in jarray)     // Read data from array.
                    {
                        Categories category = item.ToObject<Categories>();//Create's user list.
                        categories.Add(category);
                    }
                }
                categories = categories.OrderBy(x => x.Id).ToList();

                ViewBag.list = categories;// list of category
                return View(model);
            }
            else
            {
                return RedirectToAction("SignIn", "Account"); //Redirect to sigin
            }
            
        }
        public IActionResult Delete(int id)
        {
            if (IsLoggedIn())
            {
                var _sampleJsonFilePath = this.Environment.WebRootPath + "\\JsonFiles\\products.json"; //Set path from json file.
                using StreamReader reader = new(_sampleJsonFilePath); //Create reader variable read data from user.json file.
                var json = reader.ReadToEnd();//Read data.
                Product model = new Product();
                if (!string.IsNullOrEmpty(json))// check if file is not empty.
                {
                    var jarray = JArray.Parse(json);//Create array from json.
                    foreach (var item in jarray)    // Read data from array.
                    {
                        Product product = item.ToObject<Product>();//Create's user list.
                        if (product.Id == id)
                        {
                            model = product;
                            break;
                        }
                    }
                }
                reader.Close();
                List<Categories> categories = new List<Categories>();//Create's user list.
                var _sampleJsonFilePath1 = this.Environment.WebRootPath + "\\JsonFiles\\categories.json"; //Set path from json file.
                using StreamReader reader1 = new(_sampleJsonFilePath1); //Create reader variable read data from user.json file.
                var json1 = reader1.ReadToEnd();//Read data.
                if (!string.IsNullOrEmpty(json1))// check if file is not empty.
                {
                    var jarray = JArray.Parse(json1);//Create array from json.
                    foreach (var item in jarray)     // Read data from array.
                    {
                        Categories category = item.ToObject<Categories>();//Create's user list.
                        categories.Add(category);
                    }
                }
                categories = categories.OrderBy(x => x.Id).ToList();

                ViewBag.list = categories;// list of category
                return View(model);
            }
            else
            {
                return RedirectToAction("SignIn", "Account");//Redirect to signin
            }
            
        }
        [HttpPost]
        [HttpPost, ActionName("Delete")]
        public IActionResult DeleteConfirmed(int id)   //It show's view.
        {
            List<Product> products = new List<Product>();//Create's user list.
            var _sampleJsonFilePath = this.Environment.WebRootPath + "\\JsonFiles\\products.json"; //Set path from json file.
            using StreamReader reader = new(_sampleJsonFilePath); //Create reader variable read data from user.json file.
            var json = reader.ReadToEnd(); //Read data.

            if (!string.IsNullOrEmpty(json))  // check if file is not empty.
            {
                var jarray = JArray.Parse(json);      //Create array from json. 
                foreach (var item in jarray) // Read data from array.
                {
                    Product product = item.ToObject<Product>(); //Create's user list.
                    if (product.Id != id)
                    {
                        products.Add(product);
                    }
                }
            }
            reader.Close();// close reader
            string jsonData = JsonConvert.SerializeObject(products, Formatting.None);// creating json 
            System.IO.File.WriteAllText(_sampleJsonFilePath, jsonData);//  write json string to.
            return RedirectToAction("index"); // redirect to index
        }
    }
}
