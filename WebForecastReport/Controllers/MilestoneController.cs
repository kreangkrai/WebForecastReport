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
    public class MilestoneController : Controller
    {
        IMilestone Milestone;
        readonly IAccessory Accessory;

        public MilestoneController()
        {
            Milestone = new MilestoneService();
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
        public List<MilestoneModel> GetMilestones()
        {
            List<MilestoneModel> milestones = Milestone.GetMilestones();
            return milestones;
        }

        [HttpGet]
        public int GetLastMilestoneID()
        {
            int id = Milestone.GetLastMilestoneID();
            return id;
        }

        [HttpPost]
        public string CreateMilestone(string ms_str)
        {
            MilestoneModel ms = JsonConvert.DeserializeObject<MilestoneModel>(ms_str);
            string result = Milestone.CreateMilestone(ms);
            return result;
        }

        [HttpPatch]
        public string EditMilestone(string ms_str)
        {
            MilestoneModel ms = JsonConvert.DeserializeObject<MilestoneModel>(ms_str);
            string result = Milestone.EditMilestone(ms);
            return result;
        }
    }
}
