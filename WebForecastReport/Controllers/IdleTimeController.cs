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
    public class IdleTimeController : Controller
    {
        readonly IAccessory Accessory;
        readonly IWorkingHours WorkingHours;
        readonly IHoliday Holiday;

        public IdleTimeController()
        {
            this.Accessory = new AccessoryService();
            this.WorkingHours = new WorkingHoursService();
            this.Holiday = new HolidayService();
        }

        public IActionResult Index()
        {
            if (HttpContext.Session.GetString("Login_MES") != null)
            {
                string user = HttpContext.Session.GetString("userId");
                List<UserModel> users = new List<UserModel>();
                users = Accessory.getAllUser();
                UserModel u = users.Where(w => w.fullname.ToLower() == user.ToLower()).Select(s => new UserModel
                {
                    name = s.name,
                    department = s.department,
                    role = s.role,
                    section = "Eng"
                }).FirstOrDefault();
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

        public IActionResult EngineerIdleTimes()
        {
            if (HttpContext.Session.GetString("Login_MES") != null)
            {
                string user = HttpContext.Session.GetString("userId");
                List<UserModel> users = new List<UserModel>();
                users = Accessory.getAllUser();
                UserModel u = users.Where(w => w.fullname.ToLower() == user.ToLower()).Select(s => new UserModel
                {
                    name = s.name,
                    department = s.department,
                    role = s.role,
                    section = "Eng"
                }).FirstOrDefault();
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

        public IActionResult MonthlyIdleTimes()
        {
            if (HttpContext.Session.GetString("Login_MES") != null)
            {
                string user = HttpContext.Session.GetString("userId");
                List<UserModel> users = new List<UserModel>();
                users = Accessory.getAllUser();
                UserModel u = users.Where(w => w.fullname.ToLower() == user.ToLower()).Select(s => new UserModel
                {
                    name = s.name,
                    department = s.department,
                    role = s.role,
                    section = "Eng"
                }).FirstOrDefault();
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
        public List<EngineerIdleTimeModel> GetIdleTimes(DateTime startDate, DateTime stopDate)
        {
            List<WorkingHoursModel> whs = WorkingHours.GetWorkingHours().OrderBy(o => o.user_name).ThenBy(t => t.working_date).ThenBy(t => t.start_time).ToList();
            List<HolidayModel> holidays = Holiday.GetHolidays(startDate.ToString("yyyy"));
            List<EngineerIdleTimeModel> idles = new List<EngineerIdleTimeModel>();
            string[] users = whs.Select(s => s.user_name).Distinct().ToArray();
            for (int i = 0;i<users.Length;i++)
            {
                TimeSpan zeroHour = new TimeSpan(0, 0, 0, 0, 0);
                TimeSpan anHour = new TimeSpan(0, 1, 0, 0, 0);
                TimeSpan eightHours = new TimeSpan(0, 8, 0, 0, 0);
                TimeSpan aDay = new TimeSpan(0, 24, 0, 0, 0);
                TimeSpan idleTime = zeroHour;
                TimeSpan normal = zeroHour;
                TimeSpan overtime = zeroHour;
                TimeSpan businessHours = zeroHour;

                for (DateTime date = startDate; date <= stopDate; date = date.AddDays(1))
                {
                    List<WorkingHoursModel> daily = whs.Where(w => w.working_date.ToString("yyyy-MM-dd") == date.ToString("yyyy-MM-dd") && w.user_name == users[i]).ToList();
                    
                    //Check if Running Date is Holiday or Not
                    bool holiday = holidays.Where(w => w.date.ToString("yyyy-MM-dd") == date.ToString("yyyy-MM-dd")).Count() > 0 ? true : false;
                    
                    //Check if Running Date is Weekend or Not
                    bool workingDay = (date.DayOfWeek == DayOfWeek.Saturday || date.DayOfWeek == DayOfWeek.Sunday)? false : true;

                    //Add 8 Hours to Business Working Hours
                    if(!holiday && workingDay)
                    {
                        businessHours = businessHours.Add(eightHours);
                    }
                    
                    //If no WorkingHours Add 8 Hours to Idle Time
                    if (daily.Count() == 0)
                    {
                        if (!holiday && workingDay) idleTime = idleTime.Add(eightHours);
                        continue;
                    }
                    
                    bool lunch = false;
                    bool dinner = false;
                    TimeSpan hours = zeroHour;
                    
                    for(int j = 0;j<daily.Count;j++)
                    {
                        //Add 24 Hours to Stop Time if Stop Time is Less Than Start Time
                        /*if (daily[j].stop_time < daily[j].start_time)
                        {
                            daily[j].stop_time.Add(aDay);
                        }
                        hours = hours.Add(daily[j].stop_time - daily[j].start_time);*/
                        TimeSpan duration = zeroHour;
                        if(daily[j].stop_time > daily[j].start_time)
                        {
                            duration = daily[j].stop_time - daily[j].start_time;
                        } else
                        {
                            duration = daily[j].start_time - daily[j].stop_time;
                        }
                        hours = hours.Add(duration);
                        lunch = (lunch || daily[j].lunch);
                        dinner = (dinner || daily[j].dinner);
                    }

                    //Minus 1 Hour if Lunch
                    if (lunch)
                    {
                        hours = hours.Subtract(anHour);
                    }

                    //Minus 1 Hour if Dinner
                    if (dinner)
                    {
                        hours = hours.Subtract(anHour);
                    }

                    //Set Hours to 0
                    if (hours < zeroHour)
                    {
                        hours = zeroHour;
                    }

                    //Normal Working Day
                    if(!holiday && workingDay)
                    {
                        TimeSpan remain = zeroHour;
                        //If Working Hours is Less Than 8 Hours, Add Remaining Hours to Idle
                        if(hours < eightHours)
                        {
                            remain = eightHours - hours;
                            normal = hours;
                            idleTime = idleTime.Add(remain);
                        }
                        else
                        {
                            remain = hours - eightHours;
                            normal = normal.Add(eightHours);
                            overtime = overtime.Add(remain);
                        }
                    }
                    //Weekend and Holiday
                    else
                    {
                        overtime = overtime.Add(hours);
                    }
                }

                int hoursIdle = (int)(idleTime.TotalMinutes / 60);
                int hoursNormal = (int)(normal.TotalMinutes / 60);
                int hoursOvertime = (int)(overtime.TotalMinutes / 60);
                int hoursBusiness = (int)(businessHours.TotalMinutes / 60);

                EngineerIdleTimeModel idle = new EngineerIdleTimeModel()
                {
                    userName = users[i],
                    workingHours = hoursBusiness,
                    idle = hoursIdle,
                    normal = hoursNormal,
                    overtime = hoursOvertime,
                };
                idles.Add(idle);
            }
            return idles;
        }
    }
}
