﻿using System;
using System.Collections.Generic;
using System.IO;
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
        readonly IExport Export;
        private readonly IHostingEnvironment _hostingEnvironment;
        public QuotationReportController(IHostingEnvironment hostingEnvironment)
        {
            Accessory = new AccessoryService();
            Quotation_Report = new Quotation_ReportService();
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
            departments.Add("ALL");
            departments.AddRange(Accessory.GetDepartmentOfQuotation());

            List<string> years = new List<string>();
            for (int year = DateTime.Now.Year; year > DateTime.Now.Year - 5; year--)
            {
                years.Add(year.ToString());
            }

            var list = new { departments = departments, years = years };

            return Json(list);
        }

        [HttpPost]
        public JsonResult GetReportDepartment(string department, string month)
        {
            List<Quotation_Report_DepartmentModel> reports = new List<Quotation_Report_DepartmentModel>();
            reports = Quotation_Report.GetReportDepartment(department, month);
            return Json(reports);
        }
        [HttpPost]
        public JsonResult GetReportQuarter(string department, string year)
        {
            List<Quotation_Report_QuarterModel> reports = new List<Quotation_Report_QuarterModel>();
            reports = Quotation_Report.GetReportQuarter(department, year);
            return Json(reports);
        }

        public IActionResult DownloadXlsxReportDepartment(string department, string month)
        {
            //Download Excel
            var templateFileInfo = new FileInfo(Path.Combine(_hostingEnvironment.ContentRootPath, "./wwwroot/template", "quotation_report_department.xlsx"));
            var stream = Export.ExportQuotation_Report_Department(templateFileInfo, department, month);
            return File(stream, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "quotation_report_department_" + DateTime.Now.ToString("yyyy-MM-dd HH_mm_ss") + ".xlsx");
        }

        public IActionResult DownloadXlsxReportQuarter(string department, string year)
        {
            //Download Excel
            var templateFileInfo = new FileInfo(Path.Combine(_hostingEnvironment.ContentRootPath, "./wwwroot/template", "quotation_report_quarter.xlsx"));
            var stream = Export.ExportQuotation_Report_Quarter(templateFileInfo, department, year);
            return File(stream, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "quotation_report_quarter_" + DateTime.Now.ToString("yyyy-MM-dd HH_mm_ss") + ".xlsx");
        }
    }
}