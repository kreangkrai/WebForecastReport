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
        List<WorkingHoursModel> GetWorkingHours(string user_name);
        List<WorkingHoursModel> GetWorkingHours(string year, string month, string user_name);
        List<WorkingHoursModel> GetWorkingHours(string user_name, DateTime working_date);
        string AddWorkingHours(WorkingHoursModel wh);
        string UpdateWorkingHours(WorkingHoursModel wh);
        string UpdateRestTime(WorkingHoursModel wh);
        string DeleteWorkingHours(WorkingHoursModel wh);
        List<JobWeeklyWorkingHoursModel> GetAllJobWorkingHours(int year, int week);
    }
}
