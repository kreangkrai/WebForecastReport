﻿using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebForecastReport.Interface;
using WebForecastReport.Interface.MPR;
using WebForecastReport.Models;
using WebForecastReport.Models.MPR;
using WebForecastReport.Service;
using WebForecastReport.Service.MPR;

namespace WebForecastReport.Controllers
{
    public class JobMilestoneController : Controller
    {
        IJobMilestone JobMilestone;
        readonly IAccessory Accessory;

        public JobMilestoneController()
        {
            JobMilestone = new JobMilestoneService();
            this.Accessory = new AccessoryService();
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

        [HttpGet]
        public List<JobMilestoneModel> GetJobsMilestones()
        {
            List<JobMilestoneModel> jms = JobMilestone.GetJobsMilestones();
            return jms;
        }

        [HttpGet]
        public List<JobMilestoneModel> GetJobMilestones(string jobId)
        {
            List<JobMilestoneModel> jobMilestones = JobMilestone.GetJobMilestones(jobId);
            return jobMilestones;
        }

        [HttpGet]
        public List<JobMilestoneModel> GetJobsMilestonesAfterDate(DateTime date)
        {
            List<JobMilestoneModel> jms = JobMilestone.GetJobsMilestonesAfterDate(date);
            return jms;
        }

        [HttpPost]
        public string CreateJobMilestone(string jmStr)
        {
            JobMilestoneModel jm = JsonConvert.DeserializeObject<JobMilestoneModel>(jmStr);
            string result = JobMilestone.CreateJobMilestone(jm);
            return result;
        }

        [HttpPatch]
        public string EditJobMilestone(string jmStr)
        {
            JobMilestoneModel jm = JsonConvert.DeserializeObject<JobMilestoneModel>(jmStr);
            string result = JobMilestone.EditJobMilestone(jm);
            return result;
        }

        [HttpDelete]
        public string DeleteJobMilestone(string jmStr)
        {
            JobMilestoneModel jm = JsonConvert.DeserializeObject<JobMilestoneModel>(jmStr);
            string result = JobMilestone.DeleteJobMilestone(jm);
            return result;
        }

        [HttpDelete]
        public string DeleteAllJobMilestones(string jmStr)
        {
            JobMilestoneModel jm = JsonConvert.DeserializeObject<JobMilestoneModel>(jmStr);
            string result = JobMilestone.DeleteAllJobMilestones(jm);
            return result;
        }
    }
}
