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
    public class QuotationController : Controller
    {
        readonly IQuotation Quotation;
        public QuotationController()
        {
            Quotation = new QuotationService();
        }
        public IActionResult Index()
        {
            return View();
        }
        [HttpPost]
        public JsonResult GetQuotation()
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
            quotations.proposer = "Kriangkrai.R";
            quotations.date = DateTime.Now;
            quotations.expected_date = DateTime.Now;
            quotations.expected_order_date = DateTime.Now;
            quotations.required_onsite_date = DateTime.Now;
            string message = Quotation.InsertQuotation(quotations);

            List<QuotationModel> getQuotation = new List<QuotationModel>();
            getQuotation = Quotation.GetAll("Kriangkrai.R");

            return Json(getQuotation);
        }
        [HttpPost]
        public JsonResult GetData()
        {
            List<QuotationModel> quotations = new List<QuotationModel>();
            quotations = Quotation.GetAll("Kriangkrai.R");
            return Json(quotations);
        }
    }
}
