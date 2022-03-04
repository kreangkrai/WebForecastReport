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
    public class MonthlyWorkingHoursController : Controller
    {
        readonly IAccessory Accessory;
        readonly ICalculateWorkingHours CalculateOTService;
        readonly IWorkingHours WorkingHoursService;
        static List<WorkingHoursModel> monthly = new List<WorkingHoursModel>();
        
        public MonthlyWorkingHoursController()
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
        public JsonResult GetWorkingHours(string user_id, string month)
        {
            var yy = month.Split("-")[0];
            var mm = month.Split("-")[1];
            int number_days = DateTime.DaysInMonth(Convert.ToInt32(yy), Convert.ToInt32(mm));
            List<WorkingHoursModel> whs = WorkingHoursService.GetWorkingHours(yy, mm, user_id);
            for(int i = 0; i < whs.Count; i++)
            {
                whs[i] = CalculateOTService.CalculateOvertime(whs[i]);
            }
            List<WorkingHoursModel> total = new List<WorkingHoursModel>();
            for (int i = 1; i <= number_days; i++)
            {
                DateTime dd = new DateTime(Convert.ToInt32(yy), Convert.ToInt32(mm), i);
                int count = whs.Where(w => w.working_date == dd).Count();
                if(count > 0)
                {
                    List<WorkingHoursModel> whsd = whs.Where(w => w.working_date == dd).Select(s => s).ToList();
                    for (int j = 0; j < count; j++)
                    {
                        WorkingHoursModel wh = new WorkingHoursModel()
                        {
                            working_date = dd,
                            job_id = whsd[j].job_id,
                            job_name = whsd[j].job_name,
                            task_id = whsd[j].task_id,
                            task_name = whsd[j].task_name,
                            start_time = whsd[j].start_time,
                            stop_time = whsd[j].stop_time,
                            lunch = whsd[j].lunch,
                            dinner = whsd[j].dinner,
                            normal = whsd[j].normal,
                            ot1_5 = whsd[j].ot1_5,
                            ot3_0 = whsd[j].ot3_0,
                        };
                        total.Add(wh);
                    }
                }
                else
                {
                    WorkingHoursModel wh = new WorkingHoursModel()
                    {
                        working_date = dd,
                        job_id = "",
                        job_name = "",
                        task_id = "",
                        task_name = "",
                        start_time = default(TimeSpan),
                        stop_time = default(TimeSpan),
                        lunch = true,
                        dinner = true,
                        normal = default(TimeSpan),
                        ot1_5 = default(TimeSpan),
                        ot3_0 = default(TimeSpan)
                    };
                    total.Add(wh);
                }
            }
            monthly = total;
            return Json(total);
        }

        [HttpGet]
        public JsonResult GetMonthlySummary()
        {
            List<string> job_ids = monthly.Select(s => s.job_id).Distinct().ToList();
            job_ids = job_ids.Where(w => w != null).ToList();
            List<JobWorkingHoursSummaryModel> jwhs = new List<JobWorkingHoursSummaryModel>();

            TimeSpan total_normal = new TimeSpan();
            TimeSpan total_ot1_5 = new TimeSpan();
            TimeSpan total_ot3_0 = new TimeSpan();
            for(int i = 0; i < job_ids.Count; i++)
            {
                TimeSpan nn = new TimeSpan();
                TimeSpan ot1_5 = new TimeSpan();
                TimeSpan ot3_0 = new TimeSpan();
                List<WorkingHoursModel> job = monthly.Where(w => w.job_id == job_ids[i]).ToList();
                for(int j = 0; j < job.Count; j++)
                {
                    nn += job[j].normal;
                    ot1_5 += job[j].ot1_5;
                    ot3_0 += job[j].ot3_0;
                }
                total_normal += nn;
                total_ot1_5 += ot1_5;
                total_ot3_0 += ot3_0;
                JobWorkingHoursSummaryModel jwh = new JobWorkingHoursSummaryModel()
                {
                    job_id = job_ids[i],
                    job_name = monthly.Where(w => w.job_id == job_ids[i]).Select(s => s.job_name).FirstOrDefault().ToString(),
                    normal_hours = Convert.ToInt32(Math.Floor(nn.TotalHours)),
                    normal_min = Convert.ToInt32(nn.Minutes),
                    ot1_5_hours = Convert.ToInt32(Math.Floor(ot1_5.TotalHours)),
                    ot1_5_min = Convert.ToInt32(ot1_5.Minutes),
                    ot3_0_hours = Convert.ToInt32(Math.Floor(ot3_0.TotalHours)),
                    ot3_0_min = Convert.ToInt32(ot3_0.Minutes)
                };
                jwhs.Add(jwh);
            }
            jwhs.Add(new JobWorkingHoursSummaryModel()
            {
                job_id = "Total",
                job_name = "Total",
                normal_hours = Convert.ToInt32(total_normal.TotalHours),
                normal_min = Convert.ToInt32(total_normal.Minutes),
                ot1_5_hours = Convert.ToInt32(total_ot1_5.TotalHours),
                ot1_5_min = Convert.ToInt32(total_ot1_5.Minutes),
                ot3_0_hours = Convert.ToInt32(total_ot3_0.TotalHours),
                ot3_0_min = Convert.ToInt32(total_ot3_0.Minutes),
            });
            jwhs.Where(w => w.job_id != null).Select(s => s).ToList();
            return Json(jwhs);
        }
    }
}
