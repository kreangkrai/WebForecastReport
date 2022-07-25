﻿using Microsoft.AspNetCore.Http;
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
        readonly ITask Task;
        readonly IWorkingHours WorkingHours;

        public WeeklySummaryController()
        {
            this.Accessory = new AccessoryService();
            this.Task = new TaskService();
            this.WorkingHours = new WorkingHoursService();
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
        public JsonResult GetWorkingHours(string week)
        {
            List<WorkingHoursModel> whs = WorkingHours.GetWorkingHours(Convert.ToInt32(week.Split("-")[0]), Convert.ToInt32(week.Split("W")[1]));
            string[] engineers = whs.Select(s => s.user_id).Distinct().ToArray();
            string[] jobs = whs.Select(s => s.job_id).Distinct().ToArray();
            string[] tasks = whs.Select(s => s.task_id).Distinct().ToArray();
            List<WeeklySummaryModel> weekly = new List<WeeklySummaryModel>();
            for(int i = 0; i < engineers.Count(); i++)
            {
                for(int j = 0;j<jobs.Count();j++)
                {
                    for(int k = 0;k<tasks.Count();k++)
                    {
                        List<WorkingHoursModel> wh = whs.Where(w => w.user_id == engineers[i] && w.job_id == jobs[j] && w.task_id == tasks[k]).ToList();
                        int hours = 0;
                        if(wh.Count > 0)
                        {
                            hours = wh.Sum(s => Convert.ToInt32((s.stop_time - s.start_time).TotalHours));
                        }
                        weekly.Add(new WeeklySummaryModel{
                            user_id = engineers[i],
                            user_name = whs.Where(w => w.user_id == engineers[i]).Select(s => s.user_name).FirstOrDefault(),
                            job_id = jobs[j],
                            job_name = whs.Where(w => w.job_id == jobs[j]).Select(s => s.job_name).FirstOrDefault(),
                            task_id = tasks[k],
                            task_name = whs.Where(w => w.task_id == tasks[k]).Select(s => s.task_name).FirstOrDefault(),
                            week = Convert.ToInt32(week.Split("-")[0]),
                            year = Convert.ToInt32(week.Split("W")[1]),
                            hours = hours
                        });
                    }
                }
            }
            return Json(weekly);
        }
    }
}
