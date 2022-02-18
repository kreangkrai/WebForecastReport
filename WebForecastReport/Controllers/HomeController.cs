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
    public class HomeController : Controller
    {
        readonly IAccessory Accessory;
        readonly IHome Home;
        public HomeController()
        {
            Accessory = new AccessoryService();
            Home = new HomeService();
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
        public JsonResult GetUser(string name, string department, string role)
        {
            List<SaleModel> sales = new List<SaleModel>();

            if (role == "Admin")
            {
                sales = Accessory.getUserQuotation();
            }
            else if (role != "Admin" || role != "" || role != null) //Manager or admin department
            {
                sales = Accessory.getUserQuotation().Where(w => w.department == department).ToList();
            }
            else // sale
            {
                sales = Accessory.getUserQuotation().Where(w => w.name == name).ToList();
            }

            return Json(sales);
        }

        [HttpPost]
        public JsonResult GetData(string name)
        {
            List<Home_DataModel> datas = new List<Home_DataModel>();
            datas = Home.getData(name);

            List<Home_StagesModel> stages = new List<Home_StagesModel>();
            stages = Home.getDataStages(name);

            Home_DayModel day = new Home_DayModel();
            day = Home.getDataDay(name);

            var list = new { datas = datas, stages = stages, day = day };

            return Json(list);
        }
    }
}
