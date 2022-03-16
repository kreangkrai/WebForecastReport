using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using WebForecastReport.Interface;
using WebForecastReport.Interface.MPR;
using WebForecastReport.Interfaces.MPR;
using WebForecastReport.Models;
using WebForecastReport.Models.MPR;
using WebForecastReport.Service;
using WebForecastReport.Service.MPR;
using WebForecastReport.Services.MPR;

namespace WebForecastReport.Controllers
{
    public class CalendarController : Controller
    {
        readonly IWorkingHours WorkingHoursService;
        readonly IAccessory Accessory;
        readonly IEngUser EngineerService;
        readonly IJobResponsible JobResponsibleService;

        public CalendarController()
        {
            WorkingHoursService = new WorkingHoursService();
            Accessory = new AccessoryService();
            EngineerService = new EngUserService();
            JobResponsibleService = new JobResponsibleService();
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
        public List<JobResponsibleModel> GetJobs(string user_id)
        {
            List<JobResponsibleModel> jrs = JobResponsibleService.GetJobResponsible(user_id);
            return jrs;
        }

        [HttpGet]
        public bool CheckAllowEditable(string user_id)
        {
            return EngineerService.CheckAllowEditable(user_id);
        }

        [HttpGet]
        public JsonResult GetWorkingHours(string user_id)
        {
            List<WorkingHoursModel> whs = WorkingHoursService.GetWorkingHours(user_id);
            whs = whs.OrderByDescending(w => w.working_date).ToList();
            return Json(whs);
        }

        [HttpGet] 
        public JsonResult GetEngineerUser(string user_id)
        {
            EngUserModel eng = EngineerService.GetEngineerUser(user_id);
            return Json(eng);
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

        [HttpPost]
        public JsonResult AddWorkingHoursDays(string[] wh_strings)
        {
            List<WorkingHoursModel> whs = new List<WorkingHoursModel>();
            for(int i = 0;i< wh_strings.Count(); i++)
            {
                WorkingHoursModel wh = JsonConvert.DeserializeObject<WorkingHoursModel>(wh_strings[i]);
                var result = WorkingHoursService.AddWorkingHours(wh);
            }
            return Json("Success");
        }

        [HttpPatch]
        public JsonResult EditWorkingHours(string wh_string)
        {
            WorkingHoursModel wh = JsonConvert.DeserializeObject<WorkingHoursModel>(wh_string);
            var result = WorkingHoursService.UpdateWorkingHours(wh);
            return Json(result);
        }

        [HttpDelete]
        public JsonResult DeleteWorkingHours(string wh_string)
        {
            WorkingHoursModel wh = JsonConvert.DeserializeObject<WorkingHoursModel>(wh_string);
            var result = WorkingHoursService.DeleteWorkingHours(wh);
            return Json(result);
        }
    }
}
