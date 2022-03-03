using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebForecastReport.Models.MPR;

namespace WebForecastReport.Interfaces.MPR
{
    interface IWorkingHours
    {
        List<WorkingHoursModel> GetWorkingHours();
        List<WorkingHoursModel> GetWorkingHours(string user_id);
        List<WorkingHoursModel> GetWorkingHours(string year, string month, string user);
        List<WorkingHoursModel> GetWorkingHours(string user, DateTime working_date);
        string AddWorkingHours(WorkingHoursModel wh);
        string UpdateWorkingHours(WorkingHoursModel wh);
    }
}
