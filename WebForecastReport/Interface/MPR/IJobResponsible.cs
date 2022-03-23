using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebForecastReport.Models.MPR;

namespace WebForecastReport.Interface.MPR
{
    interface IJobResponsible
    {
        List<JobResponsibleModel> GetJobResponsible(string user_id);
        List<JobResponsibleModel> GetJobLists();
        List<QuotationResponsibleModel> GetQuotationResponsible(string user_id);
        List<JobResponsibleModel> GetAssignEngineers(string job_id);
        string AddJobResponsible(JobResponsibleModel jr);
    }
}
