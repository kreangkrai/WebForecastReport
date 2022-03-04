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
    public class WorkingHoursController : Controller
    {
        readonly IAccessory Accessory;
        readonly ICalculateWorkingHours CalculateOTService;
        readonly IWorkingHours WorkingHoursService;

        public WorkingHoursController()
        {
            Accessory = new AccessoryService();
            CalculateOTService = new CalculateOvertimeService();
            WorkingHoursService = new WorkingHoursService();
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
            for(int i = 0; i < whs.Count; i++)
            {
                whs[i] = CalculateOTService.CalculateOvertime(whs[i]);
            }
            return Json(whs);
        }

        [HttpPost]
        public JsonResult AddWorkingHours(string wh_string)
        {
            WorkingHoursModel wh = JsonConvert.DeserializeObject<WorkingHoursModel>(wh_string);
            var result = WorkingHoursService.AddWorkingHours(wh);
            return Json(result);
        }
    }
}
