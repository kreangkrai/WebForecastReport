using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
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
    public class WeeklySummaryController : Controller
    {
        readonly IAccessory Accessory;
        readonly IWorkingHours WorkingHours;
        static List<WorkingHoursModel> whs;

        public WeeklySummaryController()
        {
            this.Accessory = new AccessoryService();
            this.WorkingHours = new WorkingHoursService();
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

        public IActionResult EngineerWeeklySummary()
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
        public JsonResult GetWorkingHours(string week)
        {
            whs = WorkingHours.GetWorkingHours(Convert.ToInt32(week.Split("-")[0]), Convert.ToInt32(week.Split("W")[1]));
            string[] engineers = whs.OrderBy(o => o.user_id).Select(s => s.user_id).Distinct().ToArray();
            string[] jobs = whs.OrderBy(o => o.job_id).Select(s => s.job_id).Distinct().ToArray();
            string[] tasks = whs.OrderBy(o => o.task_id).Select(s => s.task_id).Distinct().ToArray();
            List<WeeklySummaryModel> weekly = new List<WeeklySummaryModel>();
            for(int i = 0; i < tasks.Count(); i++)
            {
                for(int j = 0;j<jobs.Count();j++)
                {
                    for(int k = 0;k<engineers.Count();k++)
                    {
                        List<WorkingHoursModel> wh = whs.Where(w => w.task_id == tasks[i] && w.job_id == jobs[j] && w.user_id == engineers[k]).ToList();
                        int hours = 0;
                        if(wh.Count > 0)
                        {
                            hours = wh.Sum(s => Convert.ToInt32((s.stop_time - s.start_time).TotalHours));
                        }
                        weekly.Add(new WeeklySummaryModel{
                            user_id = engineers[k],
                            user_name = whs.Where(w => w.user_id == engineers[k]).Select(s => s.user_name).FirstOrDefault(),
                            job_id = jobs[j],
                            job_name = whs.Where(w => w.job_id == jobs[j]).Select(s => s.job_name).FirstOrDefault(),
                            task_id = tasks[i],
                            task_name = whs.Where(w => w.task_id == tasks[i]).Select(s => s.task_name).FirstOrDefault(),
                            week = Convert.ToInt32(week.Split("-")[0]),
                            year = Convert.ToInt32(week.Split("W")[1]),
                            hours = hours
                        });
                    }
                }
            }
            return Json(weekly);
        }

        [HttpGet]
        public JsonResult GetNotes()
        {
            return Json(whs);
        }
    }
}
