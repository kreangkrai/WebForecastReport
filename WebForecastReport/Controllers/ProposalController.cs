using Microsoft.AspNetCore.Http;
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
    public class ProposalController : Controller
    {
        readonly IAccessory Accessory;
        readonly IProposal Proposal;
        readonly IQuotation Quotation;
        public ProposalController()
        {
            Accessory = new AccessoryService();
            Proposal = new ProposalService();
            Quotation = new QuotationService();
        }
        public IActionResult Index()
        {
            if (HttpContext.Session.GetString("Login") != null)
            {
                string user = HttpContext.Session.GetString("userId");
                List<UserModel> users = new List<UserModel>();
                users = Accessory.getAllUser();
                UserModel u = users.Where(w => w.fullname.ToLower() == user.ToLower()).Select(s => new UserModel { name = s.name, department = s.department, role = s.role }).FirstOrDefault();

                u.role = "";
                if (u.role != "Admin")  //  add propersal
                {
                    List<string> quotation = new List<string>();
                    quotation = Proposal.chkQuotation(u.name, u.role);

                    if (quotation.Count > 0) //
                    {
                        List<QuotationModel> quotations = new List<QuotationModel>();
                        quotations = Quotation.GetQuotationForProposal(u.name, u.role);

                        string message = Proposal.Insert(quotations, quotation);
                        if (message == "Insert Success")
                        {

                        }
                        else
                        {

                        }
                    }
                }
                return View(u);
            }
            else
            {
                return RedirectToAction("Index", "Account");
            }
        }

        [HttpPost]
        public JsonResult GetData(string name, string role)
        {
            List<ProposalModel> proposals = new List<ProposalModel>();
            proposals = Proposal.getProposals(name, role);

            List<string> persons = new List<string>();
            persons = Accessory.getAllUser().Select(w => w.name).ToList();
            var list = new { proposals = proposals, persons = persons };
            return Json(list);
        }
        [HttpPost]
        public JsonResult GetDepartment(string name)
        {
            string dept = Accessory.getAllUser().Where(w => w.name == name).Select(s => s.department).FirstOrDefault();
            return Json(dept);
        }
    }
}
