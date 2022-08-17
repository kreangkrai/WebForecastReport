using WebForecastReport.Interfaces.MPR;
using WebForecastReport.Models.MPR;
using WebForecastReport.Services.MPR;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using WebForecastReport.Models;
using WebForecastReport.Service;
using WebForecastReport.Interface;

namespace WebForecastReport.Controllers
{
    public class AnalysisController : Controller
    {
        readonly IAnalysis AnalysisService;
        readonly IAccessory Accessory;

        public AnalysisController()
        {
            AnalysisService = new AnalysisService();
            Accessory = new AccessoryService();
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
        public JsonResult GetTaskRatio(string job_id)
        {
            List<TaskRatioModel> trs = AnalysisService.GetTaskRatio(job_id);
            int total_hours = Convert.ToInt32(trs.Sum(s => s.hours));
            for(int i = 0; i < trs.Count(); i++)
            {
                trs[i].percents = trs[i].hours / total_hours * 100;
            }
            trs.OrderByDescending(o => o.percents);
            return Json(trs);
        }

        [HttpGet]
        public JsonResult GetTaskDistribution(string job_id)
        {
            List<TaskDistributionModel> tds = AnalysisService.GetTaskDistribution(job_id);
            tds.OrderByDescending(o => o.hours);
            return Json(tds);
        }

        [HttpGet]
        public JsonResult GetManpowerRatio(string job_id)
        {
            List<ManpowerRatioModel> mrs = AnalysisService.GetManpowerRatio(job_id);
            int total_hours = Convert.ToInt32(mrs.Sum(s => s.hours));
            mrs = mrs.GroupBy(g => g.user_id).Select(s => new ManpowerRatioModel
            {
                user_id = s.FirstOrDefault().user_id,
                user_name = s.FirstOrDefault().user_name,
                job_id = s.FirstOrDefault().job_id,
                job_name = s.FirstOrDefault().job_name,
                hours = s.Sum(su => su.hours),
                percents = s.Sum(su => su.hours) / total_hours * 100,
            }).OrderByDescending(o => o.hours).ToList();
            return Json(mrs);
        }

        [HttpGet]
        public JsonResult GetManpowerDistribution(string job_id)
        {
            List<ManpowerDistributionModel> mds = AnalysisService.GetManpowerDistribution(job_id);
            return Json(mds);
        }
    }
}
