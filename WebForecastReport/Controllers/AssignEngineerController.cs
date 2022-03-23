using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
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
    public class AssignEngineerController : Controller
    {
        readonly IWorkingHours WorkingHoursService;
        readonly IAccessory Accessory;
        readonly IEngUser EngineerService;
        readonly IJob JobService;
        readonly IJobResponsible JobResponsibleService;

        public AssignEngineerController()
        {
            WorkingHoursService = new WorkingHoursService();
            Accessory = new AccessoryService();
            EngineerService = new EngUserService();
            JobService = new JobService();
            JobResponsibleService = new JobResponsibleService();
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
        public List<JobModel> GetJobs()
        {
            List<JobModel> jobs = JobService.GetAllJobs();
            return jobs;
        }

        [HttpGet]
        public List<string> GetDepartments()
        {
            List<string> departments = EngineerService.GetUsers().Select(s => s.department).Distinct().ToList();
            return departments;
        }

        [HttpGet]
        public List<EngUserModel> GetUsers()
        {
            List<EngUserModel> users = EngineerService.GetUsers().OrderBy(o => o.user_name).ToList();
            return users;
        }

        [HttpGet]
        public List<JobResponsibleModel> GetAssignEngineers(string job_id)
        {
            List<JobResponsibleModel> jrs = JobResponsibleService.GetAssignEngineers(job_id);
            return jrs;
        }

        [HttpGet]
        public List<JobResponsibleModel> GetJobLists()
        {
            List<JobResponsibleModel> jrs = JobResponsibleService.GetJobLists();
            return jrs;
        }
    }
}
