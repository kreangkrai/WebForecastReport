using WebForecastReport.Models.MPR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebForecastReport.Interfaces.MPR
{
    interface IAnalysis
    {
        List<TaskTotalHoursModel> GetTasksWorkingHours(string job_id);
        List<JobInvolveModel> GetPercentsInvolve(string job_id);
    }
}
