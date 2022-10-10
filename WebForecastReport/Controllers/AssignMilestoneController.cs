using Microsoft.AspNetCore.Http;
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
    public class AssignMilestoneController : Controller
    {
        IAssignMilestone AssignMilestone;
        readonly IAccessory Accessory;

        public AssignMilestoneController()
        {
            AssignMilestone = new AssignMilestoneService();
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
        public List<AssignMilestoneModel> GetAssignedEngineers()
        {
            List<AssignMilestoneModel> engs = AssignMilestone.GetAssignedEngineers();
            return engs;
        }

        [HttpGet]
        public List<AssignMilestoneModel> GetEngineerAssignedJobs(string engId)
        {
            List<AssignMilestoneModel> jobs = AssignMilestone.GetEngineerAssignedJobs(engId);
            return jobs;
        }

        [HttpGet]
        public List<AssignMilestoneModel> GetJobAssignedEngineers(string jobId)
        {
            List<AssignMilestoneModel> engineers = AssignMilestone.GetJobAssignedEngineers(jobId);
            return engineers;
        }

        [HttpPost]
        public string AddEngineer(string asgStr)
        {
            AssignMilestoneModel engineer = JsonConvert.DeserializeObject<AssignMilestoneModel>(asgStr);
            string result = AssignMilestone.AddEngineer(engineer);
            return result;
        }

        [HttpPatch]
        public string EditEngineer(string asgStr)
        {
            AssignMilestoneModel engineer = JsonConvert.DeserializeObject<AssignMilestoneModel>(asgStr);
            string result = AssignMilestone.EditEngineer(engineer);
            return "Success";
        }

        [HttpDelete]
        public string DeleteEngineer(string asgStr)
        {
            AssignMilestoneModel engineer = JsonConvert.DeserializeObject<AssignMilestoneModel>(asgStr);
            string result = AssignMilestone.DeleteEngineer(engineer);
            return "Success";
        }
    }
}
