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
            if (HttpContext.Session.GetString("Login") != null)
            {
                string user = HttpContext.Session.GetString("userId");
                List<UserModel> users = new List<UserModel>();
                users = Accessory.getAllUser();
                UserModel u = users.Where(w => w.fullname.ToLower() == user.ToLower()).Select(s => new UserModel { name = s.name, department = s.department, role = s.role, section = "Sale" }).FirstOrDefault();
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
        [HttpPost]
        public JsonResult GetData()
        {
            List<UserManagementModel> users = new List<UserManagementModel>();
            users = Users.GetUsers();

            List<DepartmentModel> departments = new List<DepartmentModel>();
            departments.Add(new DepartmentModel { department = "" });
            departments.Add(new DepartmentModel { department = "Admin" });
            departments.AddRange(Accessory.getDepartment());

            List<UserModel> us = new List<UserModel>();
            us = Accessory.getAllUser();
            var list = new { us = us, users = users, roles = departments };
            return Json(list);
        }

        [HttpPost]
        public JsonResult Update(string name, string role)
        {
            string message = Users.update(name, role);
            return Json(message);
        }
        [HttpPost]
        public JsonResult Insert(string name)
        {
            if (name != "Please Select")
            {
                string message = Users.insert(name);
                return Json(message);
            }
            else
            {
                return Json("Insert Failed");
            }
        }
    }
}
