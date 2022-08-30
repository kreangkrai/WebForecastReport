using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebForecastReport.Interface;
using WebForecastReport.Interface.MPR;
using WebForecastReport.Models;
using WebForecastReport.Models.MPR;
using WebForecastReport.Service;
using WebForecastReport.Service.MPR;

namespace WebForecastReport.Controllers
{
    public class EngSystemController : Controller
    {
        readonly IAccessory Accessory;
        ISystem System;

        public EngSystemController()
        {
            this.Accessory = new AccessoryService();
            this.System = new SystemService();
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
        public List<EngSystemModel> GetSystems()
        {
            List<EngSystemModel> systems = System.GetSystems();
            return systems;
        }

        [HttpGet]
        public int GetLastSystemID()
        {
            int id = System.GetLastSystemID();
            return id;
        }

        [HttpPost]
        public JsonResult CreateSystem(string system_str)
        {
            EngSystemModel system = JsonConvert.DeserializeObject<EngSystemModel>(system_str);
            var result = System.CreateSystem(system);
            return Json(result);
        }

        [HttpPatch]
        public JsonResult EditSystem(string system_str)
        {
            EngSystemModel system = JsonConvert.DeserializeObject<EngSystemModel>(system_str);
            var result = System.EditSystem(system);
            return Json(result);
        }

        [HttpDelete]
        public JsonResult DeleteSystem(string system_str)
        {
            EngSystemModel system = JsonConvert.DeserializeObject<EngSystemModel>(system_str);
            var result = System.DeleteSystem(system);
            return Json(result);
        }
    }
}
