using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebForecastReport.Models.MPR;

namespace WebForecastReport.Interface.MPR
{
    interface IPlanManday
    {
        List<PlanMandayModel> GetJobsPlans();
        List<PlanMandayModel> GetJobPlans(string jobId);
        List<PlanMandayModel> GetEngPlans(string engId);
        List<PlanMandayModel> GetEngPlansByDate(string engId, DateTime date);
        List<PlanMandayModel> GetPlansBetweenDates(DateTime startDate, DateTime stopDate);
        string CreatePlan(PlanMandayModel plan);
        string EditPlan(PlanMandayModel plan);
        string DeletePlan(PlanMandayModel plan);
    }
}
