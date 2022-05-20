using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using WebForecastReport.Interface;
using WebForecastReport.Models;
using WebForecastReport.Service;

namespace WebForecastReport.Controllers
{
    public class LogStatusController : Controller
    {
        readonly IAccessory Accessory;
        readonly ILogStatus LogStatus;
        readonly IExport Export;
        private readonly IHostingEnvironment _hostingEnvironment;
        public LogStatusController(IHostingEnvironment hostingEnvironment)
        {
            Accessory = new AccessoryService();
            LogStatus = new LogStatusService();
            Export = new ExportService();
            _hostingEnvironment = hostingEnvironment;
        }
        public IActionResult Index()
        {
            if (HttpContext.Session.GetString("Login") != null)
            {
                string user = HttpContext.Session.GetString("userId");
                List<UserModel> users = new List<UserModel>();
                users = Accessory.getAllUser();
                UserModel u = users.Where(w => w.fullname.ToLower() == user.ToLower()).Select(s => new UserModel { name = s.name, department = s.department, role = s.role, section = "Sale" }).FirstOrDefault();
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
        [HttpPost]
        public JsonResult GetData(string year)
        {
            List<Log_StatusModel> logs = new List<Log_StatusModel>();
            logs = LogStatus.GetStatusByYear(year);

            List<string> years = new List<string>();
            for (int y = DateTime.Now.Year; y > DateTime.Now.Year - 3; y--)
            {
                years.Add(y.ToString());
            }
            var list = new { logs = logs, years = years };
            return Json(list);
        }

        public IActionResult DownloadXlsxLogStatus(string year)
        {
            //Download Excel
            var templateFileInfo = new FileInfo(Path.Combine(_hostingEnvironment.ContentRootPath, "./wwwroot/template", "LogStatus.xlsx"));
            var stream = Export.ExportLogStatus(templateFileInfo, year);
            return File(stream, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "LogStatus_" + DateTime.Now.ToString("yyyy-MM-dd HH_mm_ss") + ".xlsx");
        }
    }
}
