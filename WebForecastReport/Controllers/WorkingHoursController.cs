using WebForecastReport.Interfaces.MPR;
using WebForecastReport.Models.MPR;
using WebForecastReport.Services.MPR;
using Microsoft.AspNetCore.Mvc;
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
        readonly IWorkingHours WorkingHoursService;
        static List<WorkingHoursModel> monthly = new List<WorkingHoursModel>();
        
        public WorkingHoursController()
        {
            Accessory = new AccessoryService();
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
        public JsonResult GetWorkingHours(string user_id, string month)
        {
            int yy = Convert.ToInt32(month.Split("-")[0]);
            int mm = Convert.ToInt32(month.Split("-")[1]);
            List<WorkingHoursModel> whs = new List<WorkingHoursModel>();
            int days = DateTime.DaysInMonth(yy, mm);
            for(int i = 0;i < days; i++)
            {
                DateTime date = new DateTime(yy, mm, i + 1);
                List<WorkingHoursModel> whd = WorkingHoursService.GetWorkingHours(user_id, date);
                if(whd.Count > 0)
                {
                    whd.OrderBy(o => o.start_time);
                    for(int j = 0; j < whd.Count; j++)
                    {
                        WorkingHoursModel wh = new WorkingHoursModel();
                        wh.working_date = whd[j].working_date;
                        wh.job_id = whd[j].job_id;
                        wh.job_name = whd[j].job_name;
                        wh.task_id = whd[j].task_id;
                        wh.task_name = whd[j].task_name;
                        wh.start_time = whd[j].start_time;
                        wh.stop_time = whd[j].stop_time;
                        wh.lunch = whd[j].lunch;
                        wh.dinner = whd[j].dinner;
                        wh.wh_type = whd[j].wh_type;

                        TimeSpan hours = wh.stop_time - wh.start_time;
                        if (wh.lunch == true)
                            hours -= new TimeSpan(1, 0, 0);
                        if (wh.dinner == true)
                            hours -= new TimeSpan(1, 0, 0);

                        if (hours < default(TimeSpan))
                            hours = default(TimeSpan);

                        if (wh.wh_type == "REG")
                            wh.normal = hours;
                        else if (wh.wh_type == "OT1_5")
                            wh.ot1_5 = hours;
                        else if (wh.wh_type == "OT3")
                            wh.ot3_0 = hours;
                        else
                            wh.normal = hours;
                            
                        whs.Add(wh);
                    }
                }
                else
                {
                    WorkingHoursModel wh = new WorkingHoursModel()
                    {
                        working_date = date,
                        job_id = "",
                        job_name = "",
                        task_id = "",
                        task_name = "",
                        start_time = default(TimeSpan),
                        stop_time = default(TimeSpan),
                        lunch = false,
                        dinner = false,
                        normal = default(TimeSpan),
                        ot1_5 = default(TimeSpan),
                        ot3_0 = default(TimeSpan)
                    };
                    whs.Add(wh);
                }
            }
            monthly = whs;
            return Json(whs);
        }

        [HttpGet]
        public JsonResult GetMonthlySummary()
        {
            List<WorkingHoursSummaryModel> whs = new List<WorkingHoursSummaryModel>();
            string[] jobs = monthly.Where(w => w.job_id != "").Select(s => s.job_id).Distinct().ToArray();
            for(int i = 0; i < jobs.Count(); i++)
            {
                WorkingHoursSummaryModel js = new WorkingHoursSummaryModel();
                js.job_id = jobs[i];
                js.job_name = monthly.Where(w => w.job_id == jobs[i]).Select(s => s.job_name).FirstOrDefault();
                js.normal = Convert.ToInt32(monthly.Where(s => s.job_id == jobs[i]).Sum(t => t.normal.TotalMinutes));
                js.ot1_5 = Convert.ToInt32(monthly.Where(s => s.job_id == jobs[i]).Sum(t => t.ot1_5.TotalMinutes));
                js.ot3_0 = Convert.ToInt32(monthly.Where(s => s.job_id == jobs[i]).Sum(t => t.ot3_0.TotalMinutes));
                whs.Add(js);
            }
            return Json(whs);
        }
    }
}
