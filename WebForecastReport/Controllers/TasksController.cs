using WebForecastReport.Interfaces.MPR;
using WebForecastReport.Models.MPR;
using WebForecastReport.Services.MPR;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebForecastReport.Interface;
using WebForecastReport.Service;
using Microsoft.AspNetCore.Http;
using WebForecastReport.Models;

namespace WebForecastReport.Controllers
{
    public class TasksController : Controller
    {
        ITask TaskService;
        readonly IAccessory Accessory;

        public TasksController()
        {
            this.TaskService = new TaskService();
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

                HttpContext.Session.SetString("Role", u.role);
                HttpContext.Session.SetString("Name", u.name);
                HttpContext.Session.SetString("Department", u.department);

                return View(u);
            }
            else
            {
                return RedirectToAction("Index", "Account");
            }
        }

        [HttpGet]
        public JsonResult GetTasks()
        {
            List<TaskModel> tasks = TaskService.GetTasks();
            return Json(tasks);
        }

        [HttpPost]
        public JsonResult AddTask(string task_string)
        {
            TaskModel task = JsonConvert.DeserializeObject<TaskModel>(task_string);
            var result = TaskService.CreateTask(task);
            return Json(result);
        }

        [HttpPatch]
        public JsonResult UpdateTask(string task_string)
        {
            TaskModel task = JsonConvert.DeserializeObject<TaskModel>(task_string);
            var result = TaskService.UpdateTask(task);
            return Json(result);
        }
    }
}
