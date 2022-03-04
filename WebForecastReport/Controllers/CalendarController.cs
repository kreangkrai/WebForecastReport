using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using WebForecastReport.Interface;
using WebForecastReport.Interfaces.MPR;
using WebForecastReport.Models;
using WebForecastReport.Models.MPR;
using WebForecastReport.Service;
using WebForecastReport.Services.MPR;

namespace WebForecastReport.Controllers
{
    public class CalendarController : Controller
    {
        IWorkingHours WorkingHoursService;
        readonly IAccessory Accessory;

        public CalendarController()
        {
            this.WorkingHoursService = new WorkingHoursService();
            Accessory = new AccessoryService();
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
        public JsonResult GetWorkingHours()
        {
            List<WorkingHoursModel> whs = WorkingHoursService.GetWorkingHours();
            whs = whs.OrderByDescending(w => w.working_date).ToList();
            return Json(whs);
        }

        [HttpGet]
        public JsonResult GetWorkingHoursByDate(string user_id, DateTime working_date)
        {
            List<WorkingHoursModel> whs = WorkingHoursService.GetWorkingHours(user_id, working_date);
            whs = whs.OrderByDescending(w => w.working_date).ToList();
            return Json(whs);
        }

        [HttpPost]
        public JsonResult AddWorkingHours(string wh_string)
        {
            WorkingHoursModel wh = JsonConvert.DeserializeObject<WorkingHoursModel>(wh_string);
            var result = WorkingHoursService.AddWorkingHours(wh);
            return Json(result);
        }

        [HttpPatch]
        public JsonResult EditWorkingHours(string wh_string)
        {
            WorkingHoursModel wh = JsonConvert.DeserializeObject<WorkingHoursModel>(wh_string);
            var result = WorkingHoursService.UpdateWorkingHours(wh);
            return Json(result);
        }
    }
}
