using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebForecastReport.Interface;
using WebForecastReport.Models;
using WebForecastReport.Service;
using Microsoft.AspNetCore.Http;
using System.IO;
using Microsoft.AspNetCore.Hosting;
using System.Globalization;

namespace WebForecastReport.Controllers
{
    public class QuotationController : Controller
    {
        readonly IQuotation Quotation;
        readonly IAccessory Accessory;
        readonly IProduct Product;
        readonly IProject Project;
        readonly IService Service;
        readonly IExport Export;
        readonly ILog_Expected Log_Expected;
        readonly IUser Users;
        readonly ILogStatus LogStatus;
        readonly ILogStages LogStages;
        private readonly IHostingEnvironment _hostingEnvironment;
        public QuotationController(IHostingEnvironment hostingEnvironment)
        {
            Quotation = new QuotationService();
            Accessory = new AccessoryService();
            Product = new ProductService();
            Project = new ProjectService();
            Service = new ServiceService();
            Export = new ExportService();
            Users = new UserService();
            LogStatus = new LogStatusService();
            LogStages = new LogStagesService();
            Log_Expected = new Log_ExpectedService();
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

        public JsonResult GetQuotation(string name, string department, string role)
        {
            string quotation = Quotation.GetlastQuotation();
            QuotationModel quotations = new QuotationModel();
            string year = year = "Q" + DateTime.Now.ToString("yy"); ;
            int number = 1;
            string quo = year + number.ToString().PadLeft(4, '0');
            if (quotation != "")
            {
                year = quotation.Substring(0, 3);
                number = Convert.ToInt32(quotation.Substring(3, 4)) + 1;
                quo = year + number.ToString().PadLeft(4, '0');
            }
            quotations.quotation_no = quo;
            quotations.sale_name = name;
            quotations.department = department;
            string message = Quotation.InsertQuotation(quotations);

            List<QuotationModel> getQuotation = new List<QuotationModel>();
            getQuotation = Quotation.GetQuotation(name, role);
            getQuotation = getQuotation.OrderByDescending(o => o.quotation_no).ToList();

            var list = new { quotation = getQuotation, statepage = false };
            return Json(list);
        }
        [HttpPost]
        public JsonResult GetData(string name, string role)
        {
            List<QuotationModel> quotations = new List<QuotationModel>();
            quotations = Quotation.GetQuotation(name, role);
            quotations = quotations.OrderByDescending(o => o.quotation_no).ToList();

            //get all sale
            List<string> sales = new List<string>();
            sales.Add("");
            if (role == "Admin")
            {
                sales.AddRange(Accessory.getAllUser().Select(s => s.name).ToList());
            }
            else if (role != "Admin" && role != "" && role != null)
            {
                sales.AddRange(Accessory.getAllUser().Where(w => w.department == role).Select(s => s.name).ToList());
            }
            else
            {
                sales.AddRange(Accessory.getAllUser().Where(s => s.name == name).Select(s => s.name).ToList());
            }

            // get psm proposer
            List<string> proposer = new List<string>();
            proposer.Add("");
            proposer.AddRange(Accessory.getAllUser().Where(w=>w.department == "PSM").Select(s => s.name).ToList());

            //get all customer
            List<string> customers = new List<string>();
            customers = Accessory.getCustomers().Select(s => s.name).ToList();

            //get all end user
            List<string> endusers = new List<string>();
            endusers = Accessory.getEndUsers().Select(s => s.name).ToList();

            // get department
            List<DepartmentModel> departments = new List<DepartmentModel>();
            departments = Accessory.getDepartment();

            //get Type
            List<TypeModel> types = new List<TypeModel>();
            types = Product.GetProducts("Brand");

            // get user engineer
            List<UserManagementModel> engineer = new List<UserManagementModel>();
            //engineers.Add(new UserManagementModel() { department = "Please Select"});
            //engineers.AddRange(Accessory.getAllUser().Where(w => w.groups.Trim() == "Engineer").Select(s => s.name).ToList());
            engineer = Users.GetUsers().Where(w => w.groups.Trim() == "ENG").ToList();
            var engineers = engineer.GroupBy(g => g.department)
                .Select(s =>
                new { 
                    department = s.Key,
                    name = engineer.Where(w => w.department == s.Key).Select(a => a.name).ToList()
                }
            ).ToList();
            bool statePage = true;
            var list = new { quatations = quotations, sales = sales, customers = customers, endusers = endusers, departments = departments, types = types, statepage = statePage, engineers = engineers, proposers = proposer };
            return Json(list);
        }

        [HttpPost]
        public JsonResult Update(string user, string quotation, string revision, string date, string customer, string enduser, string project_name, string site_location, string product_type, string type, string brand, string part_no,
                   string spec, string quantity, string supplier_quotation_no, string total_value, string unit, string quoted_price, string expected_order_date, string old_expected_order_date,
                   string required_onsite_date, string proposer, string expected_date, string status, string stages, string stages_update_date, string how_to_support, string competitor, string competitor_description,
                   string competitor_price, string sale_name, string department, string detail,string engineer_in_charge,string engineer_department,bool exclude_quote, string status_changed, string check_last_status, string reason_status)
        {
            QuotationModel q = new QuotationModel()
            {
                quotation_no = quotation,
                revision = revision,
                date = date,
                customer = customer,
                enduser = enduser,
                project_name = project_name,
                site_location = site_location,
                product_type = product_type,
                type = type,
                brand = brand,
                part_no = part_no,
                spec = spec,
                quantity = quantity,
                supplier_quotation_no = supplier_quotation_no,
                total_value = total_value,
                unit = unit,
                quoted_price = quoted_price,
                expected_order_date = expected_order_date,
                required_onsite_date = required_onsite_date,
                proposer = proposer,
                expected_date = expected_date,
                status = status,
                stages = stages,
                stages_update_date = stages_update_date,
                how_to_support = how_to_support,
                competitor = competitor,
                competitor_description = competitor_description,
                competitor_price = competitor_price,
                sale_name = sale_name,
                department = department,
                detail = detail,
                engineer_in_charge = engineer_in_charge,
                engineer_department = engineer_department,
                exclude_quote = exclude_quote
            };

            Accessory.InsertCustomer(customer);
            Accessory.InsertEndUser(enduser);

            //update log expected order date
            if (old_expected_order_date != expected_order_date && old_expected_order_date != null)
            {
                Log_ExpectedModel log = new Log_ExpectedModel()
                {
                    name = user,
                    quotation = quotation,
                    project_name = project_name != null ? project_name : "",
                    date_edit = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"),
                    date_from = old_expected_order_date,
                    date_to = expected_order_date
                };
                Log_Expected.Insert(log);
            }

            //update log status, stages
            if (reason_status != "" && reason_status != null)
            {
                Log_StatusModel logs = new Log_StatusModel()
                {
                    name = user,
                    quotation = quotation,
                    project_name = project_name,
                    status_from = check_last_status,
                    status_to = status_changed,
                    date_edit = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"),
                    reason = reason_status
                };
                LogStatus.Insert(logs);
            }

            //if (reason_stages != "" && reason_stages != null)
            //{
            //    Log_StagesModel logs = new Log_StagesModel()
            //    {
            //        name = user,
            //        quotation = quotation,
            //        project_name = project_name,
            //        stages_from = check_last_stages,
            //        stages_to = stages_changed,
            //        date_edit = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"),
            //        reason = reason_stages
            //    };
            //    LogStages.Insert(logs);
            //}

            string message = Quotation.Update(q);
            bool statepage = true;
            var list = new { message = message, statepage = statepage };
            return Json(list);
        }
        [HttpPost]
        public JsonResult GetDepartment(string name)
        {
            string department = Accessory.getAllUser().Where(w => w.name == name).Select(s => s.department).FirstOrDefault();
            return Json(department);
        }
        [HttpPost]
        public JsonResult GetType(string type)
        {
            List<string> list = new List<string>();
            list.Add("Please Select");
            if (type == "Product")
            {
                list.AddRange(Product.GetProductType().Select(s => s.name).ToList());
            }
            else if (type == "Project")
            {
                list.AddRange(Project.GetProjectType().Select(s => s.name).ToList());
            }
            else if (type == "Service")
            {
                list.AddRange(Service.GetServiceType().Select(s => s.name).ToList());
            }
            return Json(list);
        }
        [HttpPost]
        public JsonResult GetBrand(string type, string type_brand)
        {
            List<string> list = new List<string>();
            list.Add("Please Select");
            if (type == "Product")
            {
                list.AddRange(Product.GetProducts(type_brand).Select(s => s.name).ToList());
            }
            else if (type == "Project")
            {
                list.AddRange(Project.GetProjects(type_brand).Select(s => s.name).ToList());
            }
            else if (type == "Service")
            {
                list.AddRange(Service.GetService(type_brand).Select(s => s.name).ToList());
            }
            return Json(list);
        }
        public IActionResult DownloadXlsxReport()
        {
            // get data
            string role = HttpContext.Session.GetString("Role");
            string name = HttpContext.Session.GetString("Name");
            string department = HttpContext.Session.GetString("Department");

            //Download Excel
            var templateFileInfo = new FileInfo(Path.Combine(_hostingEnvironment.ContentRootPath, "./wwwroot/template", "mes_quotation.xlsx"));
            var stream = Export.ExportQuotation(templateFileInfo, role, name, department);
            return File(stream, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "mes_quotation_" + DateTime.Now.ToString("yyyy-MM-dd HH_mm_ss") + ".xlsx");
        }
    }
}
