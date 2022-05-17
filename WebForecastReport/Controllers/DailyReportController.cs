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
    public class DailyReportController : Controller
    {
        readonly IAccessory Accessory;
        readonly IJobResponsible JRService;
        readonly IDRService DRService;

        public DailyReportController()
        {
            Accessory = new AccessoryService();
            JRService = new JobResponsibleService();
            DRService = new ENG_DailyReportService();
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
        public List<JobResponsibleModel> GetJobResponsible(string user_id)
        {
            List<JobResponsibleModel> jrs = JRService.GetJobResponsible(user_id);
            return jrs;
        }

        [HttpGet]
        public List<ENG_DailyReportModel> GetDailyReport(string user_id, string month, string job_id)
        {
            List<ENG_DailyReportModel> dlrs = DRService.GetDailyReport(user_id, month, job_id);
            return dlrs;
        }

        [HttpPost]
        public string AddDailyReport(string dr_str)
        {
            ENG_DailyReportModel dlr = JsonConvert.DeserializeObject<ENG_DailyReportModel>(dr_str);
            var result = DRService.AddDailyReport(dlr);
            return result;
        }

        [HttpPatch]
        public string EditDailyReport(string dr_str)
        {
            ENG_DailyReportModel dlr = JsonConvert.DeserializeObject<ENG_DailyReportModel>(dr_str);
            var result = DRService.EditDailyReport(dlr);
            return result;
        }
    }
}
