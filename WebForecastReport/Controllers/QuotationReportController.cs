using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using WebForecastReport.Interface;
using WebForecastReport.Models;
using WebForecastReport.Service;

namespace WebForecastReport.Controllers
{
    public class QuotationReportController : Controller
    {
        readonly IAccessory Accessory;
        readonly IQuotation_Report Quotation_Report;
        private readonly IHostingEnvironment _hostingEnvironment;
        public QuotationReportController(IHostingEnvironment hostingEnvironment)
        {
            Accessory = new AccessoryService();
            Quotation_Report = new Quotation_ReportService();
            _hostingEnvironment = hostingEnvironment;
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

        [HttpPost]
        public JsonResult GetDepartment()
        {
            List<string> departments = new List<string>();
            departments = Accessory.GetDepartmentOfQuotation();
            return Json(departments);
        }

        [HttpPost]
        public JsonResult GetReportDepartment(string department, string month)
        {
            List<Quotation_Report_DepartmentModel> reports = new List<Quotation_Report_DepartmentModel>();
            reports = Quotation_Report.GetReportDepartment(department, month);
            return Json(reports);
        }
    }
}
