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
using Newtonsoft.Json;

namespace WebForecastReport.Controllers
{
    public class WorkingHoursController : Controller
    {
        readonly IAccessory Accessory;
        readonly IWorkingHours WorkingHoursService;
        readonly IHoliday HolidayService;
        static List<WorkingHoursModel> monthly = new List<WorkingHoursModel>();
        
        public WorkingHoursController()
        {
            Accessory = new AccessoryService();
            WorkingHoursService = new WorkingHoursService();
            HolidayService = new HolidayService();
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
            monthly = new List<WorkingHoursModel>();
            int yy = Convert.ToInt32(month.Split("-")[0]);
            int mm = Convert.ToInt32(month.Split("-")[1]);

            List<WorkingHoursModel> whs = WorkingHoursService.GetWorkingHours(yy.ToString(), mm.ToString().PadLeft(2,'0'), user_id);
            List<HolidayModel> holidays = HolidayService.GetHolidays();

            whs = whs.OrderBy(o => o.working_date).ToList();
            int days = DateTime.DaysInMonth(yy, mm);
            for(int i = 0;i < days; i++)
            {
                DateTime date = new DateTime(yy, mm, i + 1);
                List<WorkingHoursModel> whd = whs.Where(w => w.working_date == date).ToList();
                if(whd.Count > 0)
                {
                    TimeSpan substraction = new TimeSpan(8, 0, 0);

                    TimeSpan noon = new TimeSpan(12, 0, 0);
                    TimeSpan after_noon = new TimeSpan(13, 0, 0);

                    TimeSpan evening = new TimeSpan(17, 30, 0);
                    TimeSpan end_evening = new TimeSpan(18, 30, 0);


                    bool isHoliday = holidays.Where(w => w.date == whd[0].working_date).Count() > 0 ? true : false;
                    bool isWeekend = (whd[0].working_date.DayOfWeek == DayOfWeek.Saturday || whd[0].working_date.DayOfWeek == DayOfWeek.Sunday) ? true : false;
                    whd.OrderBy(o => o.start_time);
                    for (int j = 0; j < whd.Count; j++)
                    {
                        TimeSpan regular = new TimeSpan();
                        TimeSpan ot15 = new TimeSpan();
                        TimeSpan ot3 = new TimeSpan();
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

                        if(isHoliday || isWeekend)
                        {
                            ot15 += wh.stop_time - wh.start_time;
                            if (wh.lunch && wh.start_time <= noon && wh.stop_time > after_noon)
                            {
                                ot15 -= after_noon - noon;
                            }

                            if (wh.dinner && wh.start_time <= evening && wh.stop_time > end_evening)
                            {
                                ot15 -= end_evening - evening;
                            }
                        }
                        else
                        {
                            if(wh.start_time < evening && wh.stop_time > end_evening)
                            {
                                regular += evening - wh.start_time;
                                ot15 += wh.stop_time - evening;
                            }
                            else if(wh.start_time < evening && wh.stop_time <= evening)
                            {
                                regular += wh.stop_time - wh.start_time;
                            }
                            else
                            {
                                ot15 += wh.stop_time - wh.start_time;
                            }

                            if (wh.lunch && wh.start_time <= noon && wh.stop_time > after_noon && regular.Hours > 1)
                            {
                                regular -= after_noon - noon;
                            }

                            if (wh.dinner && wh.start_time <= evening && wh.stop_time > end_evening && ot15.Hours > 1)
                            {
                                ot15 -= end_evening - evening;
                            }
                        }

                        if(ot15 > substraction)
                        {
                            ot3 += ot15 - substraction;
                            ot15 -= ot15 - substraction;
                        }

                        wh.normal = regular;
                        wh.ot1_5 = ot15;
                        wh.ot3_0 = ot3;

                        monthly.Add(wh);
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
                    monthly.Add(wh);
                }
            }
            return Json(monthly);
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

        [HttpPatch]
        public JsonResult UpdateRestTime(string task_str)
        {
            WorkingHoursModel wh = JsonConvert.DeserializeObject<WorkingHoursModel>(task_str);
            var result = WorkingHoursService.UpdateRestTime(wh);
            return Json(result);
        }
    }
}
