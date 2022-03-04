﻿using Microsoft.AspNetCore.Http;
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
    public class MainController : Controller
    {
        readonly IAccessory Accessory;

        public MainController()
        {
            Accessory = new AccessoryService();
        }

        public IActionResult Index()
        {
            if (HttpContext.Session.GetString("Login") != null)
            {
                string user = HttpContext.Session.GetString("userId");
                List<UserModel> users = new List<UserModel>();
                users = Accessory.getAllUser();
                UserModel u = users.Where(w => w.fullname.ToLower() == user.ToLower()).Select(s => new UserModel { name = s.name, department = s.department, role = s.role }).FirstOrDefault();
                if(u.role == "Admin")
                {
                    return View(u);
                }
                else if(u.role == "ENG")
                {
                    return RedirectToAction("Index", "Calendar");
                }
                else
                {
                    return RedirectToAction("Index", "Home");
                }
            }
            else
            {
                return RedirectToAction("Index", "Account");
            }
        }
    }
}
