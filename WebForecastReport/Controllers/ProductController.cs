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
        readonly IProject Project;
        readonly IService Service;
        readonly IAccessory Accessory;
        public ProductController()
        {
            Product = new ProductService();
            Project = new ProjectService();
            Service = new ServiceService();
            Accessory = new AccessoryService();
        }
        public IActionResult Index()
        {
            if (HttpContext.Session.GetString("Login") != null)
            {
                string user = HttpContext.Session.GetString("userId");
                List<UserModel> users = new List<UserModel>();
                users = Accessory.getAllUser();
                UserModel u = users.Where(w => w.fullname.ToLower() == user.ToLower()).Select(s => new UserModel { name = s.name, department = s.department, role = s.role }).FirstOrDefault();

                return View(u);
            }
            else
            {
                return RedirectToAction("Index", "Account");
            }
        }
        [HttpPost]
        public JsonResult GetData(string type, string type_brand)
        {
            if (type == "Product")
            {
                List<TypeModel> products = new List<TypeModel>();
                products = Product.GetProducts(type_brand);
                return Json(products);
            }
            else if (type == "Project")
            {
                List<ProjectModel> projects = new List<ProjectModel>();
                projects = Project.GetProjects(type_brand);
                return Json(projects);
            }
            else if (type == "Service")
            {
                List<ServiceModel> services = new List<ServiceModel>();
                services = Service.GetService(type_brand);
                return Json(services);
            }
            else
            {
                return Json(null);
            }

        }

        [HttpPost]
        public JsonResult Update(int id, string name, string type, string type_brand)
        {
            if (type == "Product")
            {
                string message = Product.Update(id, name, type_brand);
                return Json(message);
            }
            else if (type == "Project")
            {
                string message = Project.Update(id, name, type_brand);
                return Json(message);
            }
            else if (type == "Service")
            {
                string message = Service.Update(id, name, type_brand);
                return Json(message);
            }
            else
            {
                return Json(null);
            }

        }
        [HttpPost]
        public JsonResult Insert(string name, string type, string type_brand)
        {
            if (type == "Product")
            {
                if (name != "")
                {
                    string message = Product.Insert(name, type_brand);
                    return Json(message);
                }
                else
                {
                    return Json("Insert Failed");
                }
            }
            else if (type == "Project")
            {
                if (name != "")
                {
                    string message = Project.Insert(name, type_brand);
                    return Json(message);
                }
                else
                {
                    return Json("Insert Failed");
                }
            }
            else if (type == "Service")
            {
                if (name != "")
                {
                    string message = Service.Insert(name, type_brand);
                    return Json(message);
                }
                else
                {
                    return Json("Insert Failed");
                }
            }
            else
            {
                return Json(null);
            }

        }
        [HttpPost]
        public JsonResult Delete(string name, string type, string type_brand)
        {
            if (type == "Product")
            {
                string message = Product.Delete(name, type_brand);
                return Json(message);
            }
            else if (type == "Project")
            {
                string message = Project.Delete(name, type_brand);
                return Json(message);
            }
            else if (type == "Service")
            {
                string message = Service.Delete(name, type_brand);
                return Json(message);
            }
            else
            {
                return Json(null);
            }

        }
    }
}
