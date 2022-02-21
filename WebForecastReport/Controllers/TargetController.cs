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
    public class TargetController : Controller
    {
        readonly IAccessory Accessory;
        readonly ITarget Target;

        public TargetController()
        {
            Accessory = new AccessoryService();
            Target = new TargetService();
        }
        public IActionResult Index()
        {
            if (HttpContext.Session.GetString("Login") != null)
            {
                string user = HttpContext.Session.GetString("userId");
                List<UserModel> users = new List<UserModel>();
                users = Accessory.getAllUser();
                UserModel u = users.Where(w => w.fullname.ToLower() == user.ToLower()).Select(s => new UserModel { name = s.name, department = s.department, role = s.role }).FirstOrDefault();

                HttpContext.Session.SetString("Role", u.role);
                HttpContext.Session.SetString("Name", u.name);
                HttpContext.Session.SetString("Department", u.department);

                return View(u);
            }
            else
            {
                return RedirectToAction("Index", "Account");
            }
        }
        [HttpPost]
        public JsonResult GetUser()
        {
            List<SaleModel> sales = new List<SaleModel>();
            sales = Accessory.getUserQuotation();
            return Json(sales);
        }
        [HttpPost]
        public JsonResult GetDepartment(string name)
        {
            List<SaleModel> sale = new List<SaleModel>();
            sale = Accessory.getUserQuotation().Where(w => w.name == name).ToList();
            return Json(sale);
        }

        [HttpPost]
        public JsonResult GetData()
        {
            List<TargetModel> datas = new List<TargetModel>();
            datas = Target.getData();
            return Json(datas);
        }
        [HttpPost]
        public JsonResult Insert(string department, string name)
        {
            string message = Target.Insert(department, name);
            return Json(message);
        }

        [HttpPost]
        public JsonResult Delete(string name)
        {
            string message = Target.Delete(name);
            return Json(message);
        }

        [HttpPost]
        public JsonResult Update(string name, string product, string project, string service)
        {
            TargetModel target = new TargetModel()
            {
                sale_name = name,
                product = product,
                project = project,
                service = service
            };
            string message = Target.Update(target);
            return Json(message);
        }

    }
}
