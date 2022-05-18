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
                UserModel u = users.Where(w => w.fullname.ToLower() == user.ToLower()).Select(s => new UserModel { 
                    name = s.name, 
                    fullname = s.fullname,
                    department = s.department, 
                    role = s.role }).FirstOrDefault();
                HttpContext.Session.SetString("Name", u.name);
                HttpContext.Session.SetString("Fullname", u.fullname);
                HttpContext.Session.SetString("Department", u.department);
                HttpContext.Session.SetString("Role", u.role);
                return View(u);
            }
            else
            {
                return RedirectToAction("Index", "Account");
            }
        }

        [HttpGet]
        public List<JobResponsibleModel> GetJobs(string user_name)
        {
            List<JobResponsibleModel> jrs = JobResponsibleService.GetJobResponsible(user_name);
            return jrs;
        }

        [HttpGet]
        public List<QuotationResponsibleModel> GetQuotations(string user_name)
        {
            List<QuotationResponsibleModel> qrs = JobResponsibleService.GetQuotationResponsible(user_name);
            return qrs;
        }

        [HttpGet]
        public bool CheckAllowEditable(string user_name)
        {
            bool allow = EngineerService.CheckAllowEditable(user_name);
            return allow;
        }

        [HttpGet]
        public JsonResult GetWorkingHours(string user_name)
        {
            List<WorkingHoursModel> whs = WorkingHoursService.GetWorkingHours(user_name);
            whs = whs.OrderByDescending(w => w.working_date).ToList();
            return Json(whs);
        }

        [HttpGet] 
        public JsonResult GetEngineerUser(string user_name)
        {
            EngUserModel eng = EngineerService.GetEngineerUser(user_name);
            return Json(eng);
        }

        [HttpGet]
        public JsonResult GetWorkingHoursByDate(string user_name, DateTime working_date)
        {
            List<WorkingHoursModel> whs = WorkingHoursService.GetWorkingHours(user_name, working_date);
            whs = whs.OrderByDescending(w => w.working_date).ToList();
            return Json(whs);
        }

        [HttpPost]
        public JsonResult AddWorkingHours(string wh_string)
        {
            try
            {
                WorkingHoursModel wh = JsonConvert.DeserializeObject<WorkingHoursModel>(wh_string);
                
                TimeSpan noon = new TimeSpan(12, 0, 0);
                TimeSpan after_noon = new TimeSpan(13, 0, 0);
                
                if(wh.start_time < noon && wh.stop_time > after_noon)
                {
                    wh.lunch = true;
                }
                else
                {
                    wh.lunch = false;
                }

                TimeSpan evening = new TimeSpan(17, 30, 0);
                TimeSpan stop_break = new TimeSpan(18, 30, 0);
                if(wh.start_time <= evening && wh.stop_time > stop_break)
                {
                    wh.dinner = true;
                }
                else
                {
                    wh.dinner = false;
                }

                var result = WorkingHoursService.AddWorkingHours(wh);
                return Json(result);
            }
            catch(Exception exception)
            {
                return Json(exception.Message);
            }
        }

        [HttpPost]
        public JsonResult AddWorkingHoursDays(string[] wh_strings)
        {
            List<WorkingHoursModel> whs = new List<WorkingHoursModel>();
            for (int i = 0; i < wh_strings.Count(); i++)
            {
                WorkingHoursModel wh = JsonConvert.DeserializeObject<WorkingHoursModel>(wh_strings[i]);

                TimeSpan noon = new TimeSpan(12, 0, 0);
                TimeSpan after_noon = new TimeSpan(13, 0, 0);

                if (wh.start_time < noon && wh.stop_time > after_noon)
                {
                    wh.lunch = true;
                }
                else
                {
                    wh.lunch = false;
                }

                TimeSpan evening = new TimeSpan(17, 30, 0);
                TimeSpan stop_break = new TimeSpan(18, 30, 0);
                if (wh.start_time <= evening && wh.stop_time > stop_break)
                {
                    wh.dinner = true;
                }
                else
                {
                    wh.dinner = false;
                }

                var result = WorkingHoursService.AddWorkingHours(wh);
            }
            return Json("Success");
        }

        [HttpPatch]
        public JsonResult EditWorkingHours(string wh_string)
        {
            try
            {
                WorkingHoursModel wh = JsonConvert.DeserializeObject<WorkingHoursModel>(wh_string);
                var result = WorkingHoursService.UpdateWorkingHours(wh);
                return Json(result);
            }
            catch(Exception exception)
            {
                return Json(exception.Message);
            }
        }

        [HttpDelete]
        public JsonResult DeleteWorkingHours(string wh_string)
        {
            try
            {
                WorkingHoursModel wh = JsonConvert.DeserializeObject<WorkingHoursModel>(wh_string);
                var result = WorkingHoursService.DeleteWorkingHours(wh);
                return Json(result);
            }
            catch(Exception exception)
            {
                return Json(exception.Message);
            }
        }
    }
}
