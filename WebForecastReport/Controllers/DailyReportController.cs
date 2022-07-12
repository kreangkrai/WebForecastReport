using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;
using Rotativa.AspNetCore;
using System;
using System.Collections.Generic;
using System.IO;
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
    public class DailyReportController : Controller
    {
        readonly IAccessory Accessory;
        readonly IDailyReport DailyReport;

        static Form_DailyReportModel form_model;

        public DailyReportController()
        {
            Accessory = new AccessoryService();
            DailyReport = new DailyReportService();
        }

        public IActionResult Index()
        {
            if (HttpContext.Session.GetString("Login") != null)
            {
                string user = HttpContext.Session.GetString("userId");
                List<UserModel> users = new List<UserModel>();
                users = Accessory.getAllUser();
                UserModel u = users.Where(w => w.fullname.ToLower() == user.ToLower()).Select(s => new UserModel { 
                    name = s.name, 
                    fullname = s.fullname,
                    department = s.department, 
                    role = s.role }).FirstOrDefault();
                HttpContext.Session.SetString("Name", u.name);
                HttpContext.Session.SetString("Fullname", u.fullname);
                HttpContext.Session.SetString("Role", u.role);
                HttpContext.Session.SetString("Department", u.department);
                return View(u);
            }
            else
            {
                return RedirectToAction("Index", "Account");
            }
        }

        [HttpGet]
        public List<DailyActivityModel> GetDailyActivities(string user_name, DateTime start_date, DateTime stop_date)
        {
            List<DailyActivityModel> drs = DailyReport.GetDailyActivities(user_name, start_date, stop_date);
            form_model = new Form_DailyReportModel()
            {
                name = user_name,
                start_date = start_date,
                stop_date = stop_date,
                datas = drs
            };
            return drs;
        }

        [HttpPatch]
        public JsonResult UpdateActivity(string activity_string)
        {
            DailyActivityModel da = JsonConvert.DeserializeObject<DailyActivityModel>(activity_string);
            var result = DailyReport.EditDailyReport(da);
            return Json(result);
        }

        public IActionResult FormDailyReport(string user_name, DateTime start_date, DateTime stop_date)
        {
            string footer = "" +
                "--print-media-type " + 
                "--footer-left \"Job No : J99-9999\" " +
                "--footer-center \"Page 1 of 1\" " + 
                "--footer-right \"Report Date : 08-07-2022\" " +
                "--footer-font-size \"14\"";
            var form_dailyreport = new ViewAsPdf("FormDailyReport")
            {
                Model = form_model,
                PageOrientation = Rotativa.AspNetCore.Options.Orientation.Landscape,
                PageSize = Rotativa.AspNetCore.Options.Size.A4,
                /*CustomSwitches = footer*/
            };
            return form_dailyreport;
        }

        [HttpGet]
        public ActionResult Export(string user_name, DateTime start_date, DateTime stop_date)
        {
            List<DailyActivityModel> drs = DailyReport.GetDailyActivities(user_name, start_date, stop_date);
            string sFileName = @"DailyReport.xlsx";

            IWorkbook workbook = new XSSFWorkbook();
            ISheet excelSheet = workbook.CreateSheet("DailyReport");

            ICellStyle HeaderStyle = workbook.CreateCellStyle();
            HeaderStyle.Alignment = HorizontalAlignment.Center;
            HeaderStyle.VerticalAlignment = VerticalAlignment.Center;
            HeaderStyle.FillForegroundColor = IndexedColors.LightOrange.Index;
            HeaderStyle.FillPattern = FillPattern.SolidForeground;
            IFont bold_font = workbook.CreateFont();
            bold_font.IsBold = true;
            HeaderStyle.SetFont(bold_font);

            IRow row = excelSheet.CreateRow(0);

            ICell Header = row.CreateCell(0, CellType.String);
            Header.SetCellValue("Date");
            Header.CellStyle = HeaderStyle;

            Header = row.CreateCell(1, CellType.String);
            Header.SetCellValue("Start");
            Header.CellStyle = HeaderStyle;

            Header = row.CreateCell(2, CellType.String);
            Header.SetCellValue("Stop");
            Header.CellStyle = HeaderStyle;

            Header = row.CreateCell(3, CellType.String);
            Header.SetCellValue("Activity");
            Header.CellStyle = HeaderStyle;

            Header = row.CreateCell(4, CellType.String);
            Header.SetCellValue("Problem");
            Header.CellStyle = HeaderStyle;

            Header = row.CreateCell(5, CellType.String);
            Header.SetCellValue("Solution");
            Header.CellStyle = HeaderStyle;

            Header = row.CreateCell(6, CellType.String);
            Header.SetCellValue("Tomorrow Plan");
            Header.CellStyle = HeaderStyle;

            Header = row.CreateCell(7, CellType.String);
            Header.SetCellValue("Action By");
            Header.CellStyle = HeaderStyle;

            Header = row.CreateCell(8, CellType.String);
            Header.SetCellValue("Customer");
            Header.CellStyle = HeaderStyle;

            for (int i = 0; i < drs.Count(); i++)
            {
                row = excelSheet.CreateRow(i + 1);
                row.CreateCell(0, CellType.Numeric).SetCellValue("999");
                row.CreateCell(1, CellType.Numeric).SetCellValue("111");
                row.CreateCell(2, CellType.Numeric).SetCellValue("333");
                
                /*row.CreateCell(0, CellType.String).SetCellValue(Convert.ToString(drs[i].date));
                row.CreateCell(1, CellType.String).SetCellValue(Convert.ToString(drs[i].start_time));
                row.CreateCell(2, CellType.String).SetCellValue(Convert.ToString(drs[i].stop_time));
                row.CreateCell(3, CellType.String).SetCellValue(drs[i].job_id + " " + drs[i].task_name);
                row.CreateCell(4, CellType.String).SetCellValue(drs[i].problem);
                row.CreateCell(5, CellType.String).SetCellValue(drs[i].solution);
                row.CreateCell(6, CellType.String).SetCellValue(drs[i].tomorrow_plan);
                row.CreateCell(7, CellType.String).SetCellValue(drs[i].user_id);
                row.CreateCell(8, CellType.String).SetCellValue(drs[i].customer);*/
            }

            using (var fs = new FileStream(Path.Combine("wwwroot/files/", sFileName), FileMode.Create, FileAccess.Write))
            {
                workbook.Write(fs);
            }

            var memory = new MemoryStream();
            using (var stream = new FileStream(Path.Combine("wwwroot/files/", sFileName), FileMode.Open))
            {
                stream.CopyTo(memory);
            }
            memory.Position = 0;

            string files = "wwwroot/files/DailyReport.xlsx";
            byte[] fileBytes = System.IO.File.ReadAllBytes(files);
            return File(fileBytes, System.Net.Mime.MediaTypeNames.Application.Octet, "DailyReport.xlsx");
        }
    }
}
