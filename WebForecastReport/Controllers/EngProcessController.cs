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
    public class EngProcessController : Controller
    {
        readonly IAccessory Accessory;
        IProcess Process;

        public EngProcessController()
        {
            this.Accessory = new AccessoryService();
            this.Process = new ProcessService();
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
        public List<EngProcessModel> GetProcesses()
        {
            List<EngProcessModel> processes = Process.GetProcesses();
            return processes;
        }

        [HttpGet]
        public int GetLastProcessID()
        {
            int id = Process.GetLastProcessID();
            return id;
        }

        [HttpPost]
        public JsonResult CreateProcess(string process_str)
        {
            EngProcessModel process = JsonConvert.DeserializeObject<EngProcessModel>(process_str);
            var result = Process.CreateProcess(process);
            return Json(result);
        }

        [HttpPatch]
        public JsonResult EditProcess(string process_str)
        {
            EngProcessModel process = JsonConvert.DeserializeObject<EngProcessModel>(process_str);
            var result = Process.EditProcess(process);
            return Json(result);
        }

        [HttpDelete]
        public JsonResult DeleteProcess(string process_str)
        {
            EngProcessModel process = JsonConvert.DeserializeObject<EngProcessModel>(process_str);
            var result = Process.DeleteProcess(process);
            return Json(result);
        }
    }
}
