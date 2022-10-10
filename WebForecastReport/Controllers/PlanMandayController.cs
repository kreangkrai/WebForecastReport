using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebForecastReport.Interface;
using WebForecastReport.Interface.MPR;
using WebForecastReport.Interfaces.MPR;
using WebForecastReport.Models;
using WebForecastReport.Models.MPR;
using WebForecastReport.Service;
using WebForecastReport.Service.MPR;
using WebForecastReport.Services.MPR;

namespace WebForecastReport.Controllers
{
    public class PlanMandayController : Controller
    {
        readonly IAccessory Accessory;
        IPlanManday PlanManday;
        IJobMilestone JobMilestone;
        IHoliday Holiday;

        public PlanMandayController()
        {
            this.Accessory = new AccessoryService();
            PlanManday = new PlanMandayService();
            JobMilestone = new JobMilestoneService();
            Holiday = new HolidayService();
        }

        public IActionResult Index()
        {
            if (HttpContext.Session.GetString("Login_MES") != null)
            {
                string user = HttpContext.Session.GetString("userId");
                List<UserModel> users = new List<UserModel>();
                users = Accessory.getAllUser();
                UserModel u = users.Where(w => w.fullname.ToLower() == user.ToLower()).Select(s => new UserModel { name = s.name, department = s.department, role = s.role, section = "Eng" }).FirstOrDefault();
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

        public IActionResult JobPlanningDetail()
        {
            if (HttpContext.Session.GetString("Login_MES") != null)
            {
                string user = HttpContext.Session.GetString("userId");
                List<UserModel> users = new List<UserModel>();
                users = Accessory.getAllUser();
                UserModel u = users.Where(w => w.fullname.ToLower() == user.ToLower()).Select(s => new UserModel { name = s.name, department = s.department, role = s.role, section = "Eng" }).FirstOrDefault();
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

        public IActionResult PlanningSummary()
        {
            if (HttpContext.Session.GetString("Login_MES") != null)
            {
                string user = HttpContext.Session.GetString("userId");
                List<UserModel> users = new List<UserModel>();
                users = Accessory.getAllUser();
                UserModel u = users.Where(w => w.fullname.ToLower() == user.ToLower()).Select(s => new UserModel { name = s.name, department = s.department, role = s.role, section = "Eng" }).FirstOrDefault();
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

        public IActionResult JobPlan()
        {
            if (HttpContext.Session.GetString("Login_MES") != null)
            {
                string user = HttpContext.Session.GetString("userId");
                List<UserModel> users = new List<UserModel>();
                users = Accessory.getAllUser();
                UserModel u = users.Where(w => w.fullname.ToLower() == user.ToLower()).Select(s => new UserModel { name = s.name, department = s.department, role = s.role, section = "Eng" }).FirstOrDefault();
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

        public IActionResult EngineerPlan()
        {
            if (HttpContext.Session.GetString("Login_MES") != null)
            {
                string user = HttpContext.Session.GetString("userId");
                List<UserModel> users = new List<UserModel>();
                users = Accessory.getAllUser();
                UserModel u = users.Where(w => w.fullname.ToLower() == user.ToLower()).Select(s => new UserModel { name = s.name, department = s.department, role = s.role, section = "Eng" }).FirstOrDefault();
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

        [HttpGet]
        public List<PlanMandayModel> GetJobsPlans()
        {
            List<PlanMandayModel> plans = PlanManday.GetJobsPlans();
            return plans;
        }

        [HttpGet]
        public List<PlanMandayModel> GetJobPlans(string jobId)
        {
            List<PlanMandayModel> plans = PlanManday.GetJobPlans(jobId);
            return plans;
        }

        [HttpGet]
        public List<PlanMandayModel> GetEngPlans(string engId)
        {
            List<PlanMandayModel> plans = PlanManday.GetEngPlans(engId);
            return plans;
        }

        [HttpGet]
        public List<PlanMandayModel> GetEngPlansByDate(string engId, DateTime date)
        {
            List<PlanMandayModel> plans = PlanManday.GetEngPlansByDate(engId, date);
            return plans;
        }

        [HttpGet]
        public List<PlanMandayModel> GetPlansBetweenDates(DateTime startDate, DateTime stopDate)
        {
            List<PlanMandayModel> plans = PlanManday.GetPlansBetweenDates(startDate, stopDate);
            return plans;
        }

        [HttpPost]
        public string CreatePlan(string planStr)
        {
            PlanMandayModel plan = JsonConvert.DeserializeObject<PlanMandayModel>(planStr);
            string result = PlanManday.CreatePlan(plan);
            return result;
        }

        [HttpPost]
        public string PlanAll(string plansStr, bool overtime, bool saturday, bool sunday, bool holiday)
        {
            List<PlanMandayModel> plans = JsonConvert.DeserializeObject<List<PlanMandayModel>>(plansStr);
            List<HolidayModel> holidays = new List<HolidayModel>();

            string[] years = plans.Select(s => s.date.ToString("yyyy")).Distinct().ToArray();
            for(int i = 0;i<years.Count();i++)
            {
                holidays.AddRange(Holiday.GetHolidays(years[i]));
            }

            for (int i = 0; i < plans.Count(); i++)
            {
                JobMilestoneModel jm = JobMilestone.GetJobMilestones(plans[i].job_id).Where(w => w.job_milestone_id == plans[i].job_milestone_id).FirstOrDefault();
                DateTime stopDate = jm.stop_date;

                //Assign Normal Days
                for (DateTime startDate = jm.start_date; startDate <= stopDate; startDate = startDate.AddDays(1))
                {
                    if (plans[i].hours <= 0) continue;
                    string day = startDate.ToString("dddd");
                    //Skip Saturday Sunday and Holiday First
                    if (day == "Saturday" || day == "Sunday" || holidays.Where(w => w.date == startDate).Count() > 0) continue;
                    List<PlanMandayModel> dayPlans = PlanManday.GetEngPlansByDate(plans[i].user_id, startDate);
                    float assignedHours = dayPlans.Sum(s => s.hours);
                    float freeHours = 8 - assignedHours;
                    if (freeHours > 0)
                    {
                        PlanMandayModel assign = new PlanMandayModel()
                        {
                            job_milestone_id = plans[i].job_milestone_id,
                            job_id = plans[i].job_id,
                            milestone_id = plans[i].milestone_id,
                            user_id = plans[i].user_id,
                            date = startDate,
                            hours = plans[i].hours >= 8 ? 8 : plans[i].hours
                        };
                        string AddResult = PlanManday.CreatePlan(assign);
                        if (AddResult == "Success")
                        {
                            plans[i].hours = plans[i].hours - assign.hours;
                        }
                    }
                }

                //Assign Normal Days (Overtime)
                if (plans[i].hours > 0 && overtime == true)
                {
                    for (DateTime startDate = jm.start_date; startDate <= stopDate; startDate = startDate.AddDays(1))
                    {
                        if (plans[i].hours <= 0) continue;
                        string day = startDate.ToString("dddd");
                        //Skip Saturday Sunday and Holiday First
                        if (day == "Saturday" || day == "Sunday" || holidays.Where(w => w.date == startDate).Count() > 0) continue;
                        List<PlanMandayModel> dayPlans = PlanManday.GetEngPlansByDate(plans[i].user_id, startDate);
                        float assignedHours = dayPlans.Sum(s => s.hours);
                        float freeHours = 12 - assignedHours;
                        if (freeHours > 0)
                        {
                            PlanMandayModel assign = new PlanMandayModel()
                            {
                                job_milestone_id = plans[i].job_milestone_id,
                                job_id = plans[i].job_id,
                                milestone_id = plans[i].milestone_id,
                                user_id = plans[i].user_id,
                                date = startDate,
                                hours = plans[i].hours >= 4 ? 4 : plans[i].hours
                            };
                            string AddResult = PlanManday.CreatePlan(assign);
                            if (AddResult == "Success")
                            {
                                plans[i].hours = plans[i].hours - assign.hours;
                            }
                        }
                    }
                }

                //Assign Saturday Normal
                if (plans[i].hours > 0 && saturday == true)
                {
                    for (DateTime startDate = jm.start_date; startDate <= stopDate; startDate = startDate.AddDays(1))
                    {
                        if (plans[i].hours <= 0) continue;
                        string day = startDate.ToString("dddd");
                        //Skip If Not Saturday and Skip Holiday
                        if (day != "Saturday" || holidays.Where(w => w.date == startDate).Count() > 0) continue;
                        List<PlanMandayModel> dayPlans = PlanManday.GetEngPlansByDate(plans[i].user_id, startDate);
                        float assignedHours = dayPlans.Sum(s => s.hours);
                        float freeHours = 8 - assignedHours;
                        if (freeHours > 0)
                        {
                            PlanMandayModel assign = new PlanMandayModel()
                            {
                                job_milestone_id = plans[i].job_milestone_id,
                                job_id = plans[i].job_id,
                                milestone_id = plans[i].milestone_id,
                                user_id = plans[i].user_id,
                                date = startDate,
                                hours = plans[i].hours >= 8 ? 8 : plans[i].hours
                            };
                            string AddResult = PlanManday.CreatePlan(assign);
                            if (AddResult == "Success")
                            {
                                plans[i].hours = plans[i].hours - assign.hours;
                            }
                        }
                    }
                }

                //Assign Saturday (Overtime)
                if (plans[i].hours > 0 && saturday == true && overtime == true)
                {
                    for (DateTime startDate = jm.start_date; startDate <= stopDate; startDate = startDate.AddDays(1))
                    {
                        if (plans[i].hours <= 0) continue;
                        string day = startDate.ToString("dddd");
                        //Skip If Not Saturday and Skip Holiday
                        if (day != "Saturday" || holidays.Where(w => w.date == startDate).Count() > 0) continue;
                        List<PlanMandayModel> dayPlans = PlanManday.GetEngPlansByDate(plans[i].user_id, startDate);
                        float assignedHours = dayPlans.Sum(s => s.hours);
                        float freeHours = 16 - assignedHours;
                        if (freeHours > 0)
                        {
                            PlanMandayModel assign = new PlanMandayModel()
                            {
                                job_milestone_id = plans[i].job_milestone_id,
                                job_id = plans[i].job_id,
                                milestone_id = plans[i].milestone_id,
                                user_id = plans[i].user_id,
                                date = startDate,
                                hours = plans[i].hours >= 8 ? 8 : plans[i].hours
                            };
                            string AddResult = PlanManday.CreatePlan(assign);
                            if (AddResult == "Success")
                            {
                                plans[i].hours = plans[i].hours - assign.hours;
                            }
                        }
                    }
                }

                //Assign Sunday Normal
                if (plans[i].hours > 0 && sunday == true)
                {
                    for (DateTime startDate = jm.start_date; startDate <= stopDate; startDate = startDate.AddDays(1))
                    {
                        if (plans[i].hours <= 0) continue;
                        string day = startDate.ToString("dddd");
                        //Skip If Not Sunday and Skip Holiday
                        if (day != "Sunday" || holidays.Where(w => w.date == startDate).Count() > 0) continue;
                        List<PlanMandayModel> dayPlans = PlanManday.GetEngPlansByDate(plans[i].user_id, startDate);
                        float assignedHours = dayPlans.Sum(s => s.hours);
                        float freeHours = 8 - assignedHours;
                        if (freeHours > 0)
                        {
                            PlanMandayModel assign = new PlanMandayModel()
                            {
                                job_milestone_id = plans[i].job_milestone_id,
                                job_id = plans[i].job_id,
                                milestone_id = plans[i].milestone_id,
                                user_id = plans[i].user_id,
                                date = startDate,
                                hours = plans[i].hours >= 8 ? 8 : plans[i].hours
                            };
                            string AddResult = PlanManday.CreatePlan(assign);
                            if (AddResult == "Success")
                            {
                                plans[i].hours = plans[i].hours - assign.hours;
                            }
                        }
                    }
                }

                //Assign Sunday (Overtime)
                if (plans[i].hours > 0 && sunday == true && overtime == true)
                {
                    for (DateTime startDate = jm.start_date; startDate <= stopDate; startDate = startDate.AddDays(1))
                    {
                        if (plans[i].hours <= 0) continue;
                        string day = startDate.ToString("dddd");
                        //Skip If Not Sunday and Skip Holiday
                        if (day != "Sunday" || holidays.Where(w => w.date == startDate).Count() > 0) continue;
                        List<PlanMandayModel> dayPlans = PlanManday.GetEngPlansByDate(plans[i].user_id, startDate);
                        float assignedHours = dayPlans.Sum(s => s.hours);
                        float freeHours = 16 - assignedHours;
                        if (freeHours > 0)
                        {
                            PlanMandayModel assign = new PlanMandayModel()
                            {
                                job_milestone_id = plans[i].job_milestone_id,
                                job_id = plans[i].job_id,
                                milestone_id = plans[i].milestone_id,
                                user_id = plans[i].user_id,
                                date = startDate,
                                hours = plans[i].hours >= 8 ? 8 : plans[i].hours
                            };
                            string AddResult = PlanManday.CreatePlan(assign);
                            if (AddResult == "Success")
                            {
                                plans[i].hours = plans[i].hours - assign.hours;
                            }
                        }
                    }
                }

                //Assign Holiday Normal
                if (plans[i].hours > 0 && holiday == true)
                {
                    for (DateTime startDate = jm.start_date; startDate <= stopDate; startDate = startDate.AddDays(1))
                    {
                        if (plans[i].hours <= 0) continue;
                        string day = startDate.ToString("dddd");
                        List<PlanMandayModel> dayPlans = PlanManday.GetEngPlansByDate(plans[i].user_id, startDate);
                        float assignedHours = dayPlans.Sum(s => s.hours);
                        float freeHours = 8 - assignedHours;
                        if (freeHours > 0)
                        {
                            PlanMandayModel assign = new PlanMandayModel()
                            {
                                job_milestone_id = plans[i].job_milestone_id,
                                job_id = plans[i].job_id,
                                milestone_id = plans[i].milestone_id,
                                user_id = plans[i].user_id,
                                date = startDate,
                                hours = plans[i].hours >= 8 ? 8 : plans[i].hours
                            };
                            string AddResult = PlanManday.CreatePlan(assign);
                            if (AddResult == "Success")
                            {
                                plans[i].hours = plans[i].hours - assign.hours;
                            }
                        }
                    }
                }

                //Assign Holiday (Overtime)
                if (plans[i].hours > 0 && holiday == true && overtime == true)
                {
                    for (DateTime startDate = jm.start_date; startDate <= stopDate; startDate = startDate.AddDays(1))
                    {
                        if (plans[i].hours <= 0) continue;
                        string day = startDate.ToString("dddd");
                        List<PlanMandayModel> dayPlans = PlanManday.GetEngPlansByDate(plans[i].user_id, startDate);
                        float assignedHours = dayPlans.Sum(s => s.hours);
                        float freeHours = 16 - assignedHours;
                        if (freeHours > 0)
                        {
                            PlanMandayModel assign = new PlanMandayModel()
                            {
                                job_milestone_id = plans[i].job_milestone_id,
                                job_id = plans[i].job_id,
                                milestone_id = plans[i].milestone_id,
                                user_id = plans[i].user_id,
                                date = startDate,
                                hours = plans[i].hours >= 8 ? 8 : plans[i].hours
                            };
                            string AddResult = PlanManday.CreatePlan(assign);
                            if (AddResult == "Success")
                            {
                                plans[i].hours = plans[i].hours - assign.hours;
                            }
                        }
                    }
                }
            }
            string result = "Success";
            return result;
        }

        [HttpPatch]
        public string EditPlan(string planStr)
        {
            PlanMandayModel plan = JsonConvert.DeserializeObject<PlanMandayModel>(planStr);
            string result = PlanManday.EditPlan(plan);
            return result;
        }

        [HttpDelete]
        public string DeletePlan(string planStr)
        {
            PlanMandayModel plan = JsonConvert.DeserializeObject<PlanMandayModel>(planStr);
            string result = PlanManday.DeletePlan(plan);
            return result;
        }
    }
}
