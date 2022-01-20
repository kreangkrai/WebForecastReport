using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebForecastReport.Interface;
using WebForecastReport.Models;
using WebForecastReport.Service;

namespace WebForecastReport.Controllers
{
    public class OpportunityController : Controller
    {
        readonly IAccessory Accessory;
        public OpportunityController()
        {
            Accessory = new AccessoryService();
        }
        public IActionResult Index()
        {
            string user = HttpContext.Session.GetString("userId");
            List<UserModel> users = new List<UserModel>();
            users = Accessory.getAllUser();
            UserModel u = users.Where(w => w.fullname.ToLower() == user.ToLower()).Select(s => new UserModel { name = s.name, department = s.department, role = s.role }).FirstOrDefault();
            return View(u);
        }
    }
}
