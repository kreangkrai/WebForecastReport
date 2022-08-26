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
    public class EngSkillController : Controller
    {
        readonly IAccessory Accessory;
        ISkill Skill;

        public EngSkillController()
        {
            this.Accessory = new AccessoryService();
            this.Skill = new SkillService();
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
        public List<EngSkillModel> GetSkills()
        {
            List<EngSkillModel> skills = Skill.GetSkills();
            return skills;
        }

        [HttpPost]
        public JsonResult CreateSkill(string skill_str)
        {
            EngSkillModel skill = JsonConvert.DeserializeObject<EngSkillModel>(skill_str);
            var result = Skill.CreateSkill(skill);
            return Json(result);
        }

        [HttpPatch]
        public JsonResult EditSkill(string skill_str)
        {
            EngSkillModel skill = JsonConvert.DeserializeObject<EngSkillModel>(skill_str);
            var result = Skill.EditSkill(skill);
            return Json(result);
        }

        [HttpDelete]
        public JsonResult DeleteSkill(string skill_str)
        {
            EngSkillModel skill = JsonConvert.DeserializeObject<EngSkillModel>(skill_str);
            var result = Skill.DeleteSkill(skill);
            return Json(result);
        }
    }
}
