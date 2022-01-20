using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using WebForecastReport.Interface;
using WebForecastReport.Models;
using WebForecastReport.Service;

namespace WebForecastReport.Controllers
{
    public class UserController : Controller
    {
        readonly IAccessory Accessory;
        readonly IUser Users;
        public UserController()
        {
            Users = new UserService();
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
            List<UserManagementModel> users = new List<UserManagementModel>();
            users = Users.GetUsers();
            return Json(users);
        }

        [HttpPost]
        public JsonResult Update(string name,string department,string role)
        {
            string message = Users.update(name, department, role);
            return Json(message);
        }
        [HttpPost]
        public JsonResult Insert(string name)
        {
            string message = Users.insert(name);
            return Json(message);
        }
    }
}
