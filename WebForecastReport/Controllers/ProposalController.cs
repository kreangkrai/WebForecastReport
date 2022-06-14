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
        readonly IUser Users;
        public ProposalController()
        {
            Accessory = new AccessoryService();
            Proposal = new ProposalService();
            Quotation = new QuotationService();
            Users = new UserService();
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
            //users.AddRange(Accessory.getAllUser().Where(w => w.groups.Trim() == "Engineer").Select(s => s.name).ToList());
            users.AddRange(Users.GetUsers().Where(w => w.groups.Trim() == "ENG").Select(s => s.name).ToList());
            return Json(users);
        }

        [HttpPost]
        public JsonResult GetData(string name, string role)
        {
            List<ProposalModel> proposals = new List<ProposalModel>();
            proposals = Proposal.getProposals(name, role);

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

            var list = new { proposals = proposals, engineers = engineers };

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
                    string gp, string finish_date, string engineer_in_charge, string engineer_department, string man_hours)
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
                engineer_in_charge = engineer_in_charge,
                engineer_department = engineer_department,
                man_hours = man_hours
            };
            string message = Proposal.Update(proposal);
            return Json(message);
        }
    }
}
