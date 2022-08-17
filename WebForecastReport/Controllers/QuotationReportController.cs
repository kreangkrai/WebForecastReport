using System;
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
            if (HttpContext.Session.GetString("Login_MES") != null)
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
        public JsonResult GetDepartment()
        {
            List<string> departments = new List<string>();

            string role = HttpContext.Session.GetString("Role");

            if (role != "Admin")
            {
                departments.Add(role); //Add Department
            }
            else
            {
                departments.Add("ALL");
                departments.AddRange(Accessory.GetDepartmentOfQuotation());
            }
            
            List<string> years = new List<string>();
            for (int year = DateTime.Now.Year; year > DateTime.Now.Year - 5; year--)
            {
                years.Add(year.ToString());
            }

            var list = new { departments = departments, years = years };

            return Json(list);
        }

        [HttpPost]
        public JsonResult GetReportDepartment(string department, string month_first, string month_last)
        {
            List<Quotation_Report_DepartmentModel> reports = new List<Quotation_Report_DepartmentModel>();
            reports = Quotation_Report.GetReportDepartment(department, month_first, month_last);
            return Json(reports);
        }

        [HttpPost]
        public JsonResult GetSales(string department)
        {
            List<string> sales = new List<string>();
            sales.Add("ALL");
            sales.AddRange(Accessory.getUserQuotation().Where(w => w.department == department).Select(s => s.name).ToList());
            return Json(sales);
        }

        [HttpPost]
        public JsonResult GetReportQuarter(string department, string year,string stages)
        {
            List<Quotation_Report_QuarterModel> reports = new List<Quotation_Report_QuarterModel>();
            reports = Quotation_Report.GetReportQuarter(department, year, stages);
            return Json(reports);
        }
        [HttpPost]
        public JsonResult GetReportYear(string department, string year)
        {
            List<Quotation_Report_YearModel> reports = new List<Quotation_Report_YearModel>();
            reports = Quotation_Report.GetReportYear(department, year);
            return Json(reports);
        }
        [HttpPost]
        public JsonResult GetReportStatus(string year, string department, string sale)
        {
            List<Quotation_Report_StatusModel> reports = new List<Quotation_Report_StatusModel>();
            reports = Quotation_Report.GetReportStatus(year, department, sale);
            return Json(reports);
        }

        [HttpPost]
        public JsonResult GetReportPendingInOut(string department, string month_first, string month_last)
        {
            List<Quotation_Report_PendingInOutModel> sales = new List<Quotation_Report_PendingInOutModel>();
            sales = Quotation_Report.GetReportPendingInOutByDepSale(department, month_first, month_last);

            List<Quotation_Report_PendingInOutModel> departments = new List<Quotation_Report_PendingInOutModel>();
            departments = Quotation_Report.GetReportPendingInOutByDepartment(department, month_first, month_last);
            var list = new { department = departments, sale = sales };
            return Json(list);
        }
        public IActionResult DownloadXlsxReportDepartment(string department, string month_first, string month_last)
        {
            //Download Excel
            var templateFileInfo = new FileInfo(Path.Combine(_hostingEnvironment.ContentRootPath, "./wwwroot/template", "quotation_report_department.xlsx"));
            var stream = Export.ExportQuotation_Report_Department(templateFileInfo, department, month_first, month_last);
            return File(stream, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "quotation_report_department_" + DateTime.Now.ToString("yyyy-MM-dd HH_mm_ss") + ".xlsx");
        }

        public IActionResult DownloadXlsxReportPendingInOut(string department, string month_first, string month_last)
        {
            //Download Excel
            var templateFileInfo = new FileInfo(Path.Combine(_hostingEnvironment.ContentRootPath, "./wwwroot/template", "quotation_report_pendinginout.xlsx"));
            var stream = Export.ExportQuotation_Report_PendingInOut(templateFileInfo, department, month_first, month_last);
            return File(stream, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "quotation_report_pendinginout" + DateTime.Now.ToString("yyyy-MM-dd HH_mm_ss") + ".xlsx");
        }
        public IActionResult DownloadXlsxReportQuarter(string department, string year,string stages)
        {
            //Download Excel
            var templateFileInfo = new FileInfo(Path.Combine(_hostingEnvironment.ContentRootPath, "./wwwroot/template", "quotation_report_quarter.xlsx"));
            var stream = Export.ExportQuotation_Report_Quarter(templateFileInfo, department, year, stages);
            return File(stream, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "quotation_report_quarter_" + DateTime.Now.ToString("yyyy-MM-dd HH_mm_ss") + ".xlsx");
        }
        public IActionResult DownloadXlsxReportYear(string department, string year)
        {
            //Download Excel
            var templateFileInfo = new FileInfo(Path.Combine(_hostingEnvironment.ContentRootPath, "./wwwroot/template", "Quotation_report_year.xlsx"));
            var stream = Export.ExportQuotation_Report_Year(templateFileInfo, department, year);
            return File(stream, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "Quotation_report_year_" + DateTime.Now.ToString("yyyy-MM-dd HH_mm_ss") + ".xlsx");
        }
    }
}
