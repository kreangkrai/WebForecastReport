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
    public class EngUserController : Controller
    {
        readonly IAccessory Accessory;
        readonly IEngUser EngUserService;

        public EngUserController()
        {
            Accessory = new AccessoryService();
            EngUserService = new EngUserService();
        }

        public IActionResult Index()
        {
            if (HttpContext.Session.GetString("Login_MES") != null)
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
            List<EngUserModel> users = EngUserService.GetUsers().OrderBy(o => o.user_id).ToList();
            return Json(users);
        }

        [HttpGet]
        public JsonResult GetEngineerUsers()
        {
            List<EngUserModel> engineers = EngUserService.GetEngineerUsers().OrderBy(o => o.user_id).ToList();
            return Json(engineers);
        }

        [HttpPost]
        public JsonResult CreateEngineerUser(string user_string)
        {
            EngUserModel eng = JsonConvert.DeserializeObject<EngUserModel>(user_string);
            eng.role = eng.role == null ? "User" : eng.role;
            var result = EngUserService.CreateEngineerUser(eng);
            return Json(result);
        }

        [HttpPatch]
        public JsonResult UpdateEngineerUser(string user_string)
        {
            EngUserModel eng = JsonConvert.DeserializeObject<EngUserModel>(user_string);
            var result = EngUserService.UpdateEngineerUser(eng);
            return Json(result);
        }
    }
}
