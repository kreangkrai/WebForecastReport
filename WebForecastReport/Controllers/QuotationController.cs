using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebForecastReport.Interface;
using WebForecastReport.Models;
using WebForecastReport.Service;
using Microsoft.AspNetCore.Http;

namespace WebForecastReport.Controllers
{
    public class QuotationController : Controller
    {
        readonly IQuotation Quotation;
        readonly IAccessory Accessory;
        public QuotationController()
        {
            Quotation = new QuotationService();
            Accessory = new AccessoryService();
        }
        public IActionResult Index()
        {
            string user = HttpContext.Session.GetString("userId");
            List<UserModel> users = new List<UserModel>();
            users = Accessory.getAllUser();
            UserModel u = users.Where(w => w.fullname.ToLower() == user.ToLower()).Select(s => new UserModel { name = s.name }).FirstOrDefault();

            return View(u);
        }
        [HttpPost]
        public JsonResult GetQuotation(string name)
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
            quotations.proposer = name;
            quotations.date = DateTime.Now.ToString("yyyy-MM-dd");
            quotations.expected_date = DateTime.Now.ToString("yyyy-MM-dd");
            quotations.expected_order_date = DateTime.Now.ToString("yyyy-MM-dd");
            quotations.required_onsite_date = DateTime.Now.ToString("yyyy-MM-dd");
            string message = Quotation.InsertQuotation(quotations);

            List<QuotationModel> getQuotation = new List<QuotationModel>();
            getQuotation = Quotation.GetAll(name);
            getQuotation = getQuotation.OrderByDescending(o => o.quotation_no).ToList();
            return Json(getQuotation);
        }
        [HttpPost]
        public JsonResult GetData(string name)
        {
            List<QuotationModel> quotations = new List<QuotationModel>();
            quotations = Quotation.GetAll(name);
            quotations = quotations.OrderByDescending(o => o.quotation_no).ToList();
            return Json(quotations);
        }
        [HttpPost]
        public JsonResult Update(string quotation, string date, string customer, string enduser, string project_name, string site_location, string product_type, string expected_order_date,
                   string required_onsite_date, string proposer, string expected_date, string status, string stages, string how_to_support, string competitor, string competitor_description,
                   string competitor_price, string sale_name, string detail)
        {
            QuotationModel q = new QuotationModel()
            {
                quotation_no = quotation,
                date = date,
                customer = customer,
                enduser = enduser,
                project_name = project_name,
                site_location = site_location,
                product_type = product_type,
                part_no = "",
                spec = "",
                quantity = "",
                supplier_quotation_no = "",
                total_value = "",
                unit = "",
                expected_order_date = expected_order_date,
                required_onsite_date = required_onsite_date,
                proposer = proposer,
                expected_date = expected_date,
                status = status,
                stages = stages,
                how_to_support = how_to_support,
                competitor = competitor,
                competitor_description = competitor_description,
                competitor_price = competitor_price,
                sale_name = sale_name,
                detail = detail
            };
            string message = Quotation.Update(q);

            return Json(message);
        }
    }
}
