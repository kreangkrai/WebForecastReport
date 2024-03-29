﻿using Microsoft.AspNetCore.Hosting;
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
    public class HomeController : Controller
    {
        readonly IAccessory Accessory;
        readonly IHome Home;
        readonly IExport Export;
        private readonly IHostingEnvironment _hostingEnvironment;
        static List<Home_DataModel> temp_data = new List<Home_DataModel>();
        static TargetIndividual temp_target_individual = new TargetIndividual();
        static TargetDepartment temp_target_department = new TargetDepartment();
        public HomeController(IHostingEnvironment hostingEnvironment)
        {
            Accessory = new AccessoryService();
            Home = new HomeService();
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
        public JsonResult GetUserDepartment(string name, string department, string role)
        {

            // Sale name
            List<SaleModel> sales = new List<SaleModel>();

            if (role == "Admin")
            {
                sales = Accessory.getUserQuotation();
            }
            else if (role != "Admin" && role != "" && role != null) //Manager or admin department
            {
                sales = Accessory.getUserQuotation().Where(w => w.department == department).ToList();
            }
            else // sale
            {
                sales = Accessory.getUserQuotation().Where(w => w.name == name).ToList();
            }

            //Department

            List<DepartmentModel> departments = new List<DepartmentModel>();

            if (role == "Admin")
            {
                departments = Accessory.getDepartmentQuotation();
            }
            else
            {
                departments = Accessory.getDepartmentQuotation().Where(w => w.department == department).ToList();
            }

            List<string> years = new List<string>();
            for (int year = DateTime.Now.Year; year > DateTime.Now.Year - 3; year--)
            {
                years.Add(year.ToString());
            }

            var list = new { sales = sales, departments = departments, years = years };
            return Json(list);
        }

        [HttpPost]
        public JsonResult GetDataByIndividual(string year, string name)
        {
            List<Home_DataModel> datas = new List<Home_DataModel>();
            datas = Home.getDataByIndividual(year, name);

            List<Home_StagesModel> stages = new List<Home_StagesModel>();
            stages = Home.getDataStagesByIndividual(year, name);

            Home_DayModel day = new Home_DayModel();
            day = Home.getDataDay(year, name);

            List<HittingRateModel> hittingRates = new List<HittingRateModel>();
            hittingRates = Home.GetHittingRateByName(year,name);

            TargetIndividual target = new TargetIndividual();
            target = Home.GetTargetIndividual(year, name);

            var list = new { datas = datas, stages = stages, day = day, hittingrates = hittingRates, target = target };

            temp_data = datas;
            temp_target_individual = target;
            return Json(list);
        }

        [HttpPost]
        public JsonResult GetDataDepartment(string year, string department)
        {
            List<Home_DataModel> datas = new List<Home_DataModel>();
            datas = Home.getDataByDepartment(year, department);

            List<Home_StagesModel> stages = new List<Home_StagesModel>();
            stages = Home.getDataStagesByDepartment(year, department);

            List<Home_DayModel> day = new List<Home_DayModel>();
            day = Home.getDataDayByDepartment(year, department);

            List<HittingRateModel> hittingRates = new List<HittingRateModel>();
            hittingRates = Home.GetHittingRateByDepartment(year,department);

            TargetDepartment target = new TargetDepartment();
            target = Home.GetTargetDepartment(year, department);

            var list = new { datas = datas, stages = stages, day = day, hittingrates = hittingRates, target = target };

            temp_data = datas;
            temp_target_department = target;
            return Json(list);
        }

        [HttpPost]
        public JsonResult GetDataPerformance(string year, string department)
        {
            List<PerformanceModel> performances = new List<PerformanceModel>();
            performances = Home.getPerformance(year, department);

            List<PerformanceModel> performances_stack = new List<PerformanceModel>();
            performances_stack = Home.getPerformanceStack(year, department);

            var list = new { performances = performances, performances_stack = performances_stack };
            return Json(list);
        }

        [HttpPost]
        public JsonResult GetStagesDay(string year, string name, string day)
        {
            List<Home_Stages_DayModel> quotations = new List<Home_Stages_DayModel>();
            quotations = Home.getDataQuotationMoreDay(year, name, day);
            return Json(quotations);
        }     

        [HttpPost]
        public JsonResult GetDataNestedPieIndividual(string year, string name, string type)
        {
            List<SubQuotationModel> datas = new List<SubQuotationModel>();
            datas = Home.GetDataSubQuotationIndividual(year, name, type);
            return Json(datas);
        }
        [HttpPost]
        public JsonResult GetDataNestedPieDepartment(string year, string department, string type)
        {
            List<SubQuotationModel> datas = new List<SubQuotationModel>();
            datas = Home.GetDataSubQuotationDepartment(year, department, type);
            return Json(datas);
        }

        [HttpPost]
        public JsonResult GetViewQuotationByNo(string quotation)
        {
            QuotationModel quo = new QuotationModel();
            quo = Home.GetViewQuotationByNo(quotation);
            return Json(new { quotation = quo });
        }

        [HttpPost]
        public JsonResult GetQuotationByPieIndividual(string year,string name,string type,string data)
        {
            List<QuotationModel> quotations = new List<QuotationModel>();
            quotations = Home.GetQuotationByPieIndividual(year, name, type, data);

            return Json(new { quotation = quotations });
        }

        [HttpPost]
        public JsonResult GetQuotationByPieDepartment(string year,string department, string type, string data)
        {
            List<QuotationModel> quotations = new List<QuotationModel>();
            quotations = Home.GetQuotationByPieDepartment(year, department, type, data);

            return Json(new { quotation = quotations });
        }
        [HttpPost]
        public JsonResult GetQuotationByBarIndividual(string year, string name,string title, string type, string data)
        {
            List<QuotationModel> quotations = new List<QuotationModel>();
            quotations = Home.GetQuotationByBarIndividual(year, name,title, type, data);

            return Json(new { quotation = quotations });
        }

        [HttpPost]
        public JsonResult GetQuotationByBarDepartment(string year, string department, string title,string type, string data)
        {
            List<QuotationModel> quotations = new List<QuotationModel>();
            quotations = Home.GetQuotationByBarDepartment(year, department, title, type, data);

            return Json(new { quotation = quotations });
        }

        public IActionResult DownloadXlsxChartIndividual(string name)
        {
            //Download Excel
            var templateFileInfo = new FileInfo(Path.Combine(_hostingEnvironment.ContentRootPath, "./wwwroot/template", "chart.xlsx"));
            var stream = Export.ExportQuotation_Report_Chart_Individual(templateFileInfo,temp_data,temp_target_individual);
            return File(stream, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "Quotation_report_chart_" + name + "_" + DateTime.Now.ToString("yyyy-MM-dd HH_mm_ss") + ".xlsx");
        }
        public IActionResult DownloadXlsxChartDepartment(string department)
        {
            //Download Excel
            var templateFileInfo = new FileInfo(Path.Combine(_hostingEnvironment.ContentRootPath, "./wwwroot/template", "chart.xlsx"));
            var stream = Export.ExportQuotation_Report_Chart_Department(templateFileInfo, temp_data, temp_target_department);
            return File(stream, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "Quotation_report_chart_" + department + "_" + DateTime.Now.ToString("yyyy-MM-dd HH_mm_ss") + ".xlsx");
        }
    }
}
