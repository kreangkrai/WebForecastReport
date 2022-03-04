using WebForecastReport.Interfaces.MPR;
using WebForecastReport.Models.MPR;
using WebForecastReport.Services.MPR;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebForecastReport.Interface;
using WebForecastReport.Service;
using Microsoft.AspNetCore.Http;
using WebForecastReport.Models;

namespace WebForecastReport.Controllers
{
    public class User2Controller : Controller
    {
        readonly IAccessory Accessory;
        readonly IUser2 UserService2;

        public User2Controller()
        {
            Accessory = new AccessoryService();
            UserService2 = new User2Service();
        }

        public IActionResult Index()
        {
            if (HttpContext.Session.GetString("Login") != null)
            {
                string user = HttpContext.Session.GetString("userId");
                List<UserModel> users = new List<UserModel>();
                users = Accessory.getAllUser();
                UserModel u = users.Where(w => w.fullname.ToLower() == user.ToLower()).Select(s => new UserModel { name = s.name, department = s.department, role = s.role, section = "Eng" }).FirstOrDefault();
                HttpContext.Session.SetString("Role", u.role);
                HttpContext.Session.SetString("Name", u.name);
                HttpContext.Session.SetString("Department", u.department);
                HttpContext.Session.SetString("Section", u.section);
                return View(u);
            }
            else
            {
                return RedirectToAction("Index", "Account");
            }
        }

        [HttpGet]
        public JsonResult GetUsers()
        {
            List<UserModel2> users = UserService2.GetUsers().OrderBy(o => o.user_id).ToList();
            return Json(users);
        }

        //[HttpPost]
        //public JsonResult CreateUser(string user_string)
        //{
        //    UserModel2 user = JsonConvert.DeserializeObject<UserModel2>(user_string);
        //    var result = UserService2.CreateUser(user);
        //    return Json(result);
        //}

        //[HttpPatch]
        //public JsonResult UpdateUser(string user_string)
        //{
        //    UserModel2 user = JsonConvert.DeserializeObject<UserModel2>(user_string);
        //    var result = UserService2.UpdateUser(user);
        //    return Json(result);
        //}
    }
}
