using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebForecastReport.Interface;
using WebForecastReport.Models;
using WebForecastReport.Service;

namespace WebForecastReport.Controllers
{
    public class ProductController : Controller
    {
        readonly IProduct Product;
        readonly IAccessory Accessory;
        public ProductController()
        {
            Product = new ProductService();
            Accessory = new AccessoryService();
        }
        public IActionResult Index()
        {
            string user = HttpContext.Session.GetString("userId");
            List<UserModel> users = new List<UserModel>();
            users = Accessory.getAllUser();
            UserModel u = users.Where(w => w.fullname.ToLower() == user.ToLower()).Select(s => new UserModel { name = s.name, department = s.department, role = s.role }).FirstOrDefault();

            return View(u);
        }
        [HttpPost]
        public JsonResult GetData()
        {
            List<ProductModel> products = new List<ProductModel>();
            products = Product.GetProducts();
            return Json(products);
        }

        [HttpPost]
        public JsonResult Update(int id,string name)
        {
            string message = Product.Update(id,name);
            return Json(message);
        }
        [HttpPost]
        public JsonResult Insert(string name)
        {
            if (name != "")
            {
                string message = Product.Insert(name);
                return Json(message);
            }
            else
            {
                return Json("Insert Failed");
            }
        }
        [HttpPost]
        public JsonResult Delete(string name)
        {
            string message = Product.Delete(name);
            return Json(message);
        }
    }
}
