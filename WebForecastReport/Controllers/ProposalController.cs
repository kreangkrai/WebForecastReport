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

                HttpContext.Session.SetString("Role", u.role);
                HttpContext.Session.SetString("Name", u.name);
                HttpContext.Session.SetString("Department", u.department);
                //u.role = "";
                if (u.role != "Admin")  //  add propersal
                {
                    List<string> quotations = new List<string>();
                    quotations = Proposal.chkQuotation(u.name, u.role);

                    //insert
                    if (quotations.Count > 0) //
                    {
                        List<QuotationModel> q = new List<QuotationModel>();
                        q = Quotation.GetQuotationForProposal(u.name, u.role);

                        Proposal.Insert(q, quotations);
                    }

                    //update
                    List<string> quotation = new List<string>();
                    quotation = Proposal.chkForUpdate(u.name, u.role);
                    if (quotation.Count > 0)
                    {
                        Proposal.UpdateName(quotation, u.name);
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
        public JsonResult GetUserEngineering()
        {
            List<string> users = new List<string>();
            users.Add("Please Select");
            users.AddRange(Accessory.getAllUser().Where(w => w.groups.Trim() == "Engineer").Select(s => s.name).ToList());
            return Json(users);
        }

        [HttpPost]
        public JsonResult GetData(string name, string role)
        {
            List<ProposalModel> proposals = new List<ProposalModel>();
            proposals = Proposal.getProposals(name, role);

            List<string> users = new List<string>();
            users.Add("Please Select");
            users.AddRange(Accessory.getAllUser().Where(w => w.groups == "Engineer").Select(s => s.name).ToList());
            var list = new { proposals = proposals, users = users };

            return Json(list);
        }
        [HttpPost]
        public JsonResult GetDepartment(string name)
        {
            string dept = Accessory.getAllUser().Where(w => w.name == name).Select(s => s.department).FirstOrDefault();
            return Json(dept);
        }

        [HttpPost]
        public JsonResult Update(string quotation, string request_date, string proposal_status, string revision, string propose_cost, string quoted_price,
                    string gp, string finish_date, string personname, string persondepartment, string manhour)
        {
            ProposalModel proposal = new ProposalModel()
            {
                quotation = new QuotationModel()
                {
                    quotation_no = quotation
                },
                request_date = request_date,
                proposal_status = proposal_status,
                proposal_revision = revision,
                proposal_cost = propose_cost,
                proposal_quoted_price = quoted_price,
                gp = gp,
                finish_date = finish_date,
                engineer_in_charge = personname,
                engineer_department = persondepartment,
                man_hours = manhour
            };
            string message = Proposal.Update(proposal);
            return Json(message);
        }
    }
}
