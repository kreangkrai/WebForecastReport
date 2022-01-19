using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using WebForecastReport.Interface;
using WebForecastReport.Models;
using WebForecastReport.Service;

namespace WebForecastReport.Controllers
{
    public class UserController : Controller
    {
        readonly IUser Users;
        public UserController()
        {
            Users = new UserService();
        }
        public IActionResult Index()
        {
            return View();
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
