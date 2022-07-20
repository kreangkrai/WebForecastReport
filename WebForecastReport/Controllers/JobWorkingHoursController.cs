﻿using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
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
    class Week
    {
        [JsonProperty("year")]
        public string year { get; set; }
        [JsonProperty("week")]
        public string week { get; set; }
    }

    public class JobWorkingHoursController : Controller
    {
        readonly IAccessory Accessory;
        readonly IWorkingHours WorkingHours;

        public JobWorkingHoursController()
        {
            Accessory = new AccessoryService();
            WorkingHours = new WorkingHoursService();
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
        public JsonResult GetWorkingHours(string weeks)
        {
            List<Week> ww = JsonConvert.DeserializeObject<List<Week>>(weeks);
            List<JobWeeklyWorkingHoursModel> whs = new List<JobWeeklyWorkingHoursModel>();
            for(int i = 0;i<ww.Count;i++)
            {
                whs.AddRange(WorkingHours.GetAllJobWorkingHours(Convert.ToInt32(ww[i].year), Convert.ToInt32(ww[i].week)));
            }
            return Json(whs);
        }
    }
}
