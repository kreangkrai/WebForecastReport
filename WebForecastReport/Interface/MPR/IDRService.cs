using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebForecastReport.Models.MPR;

namespace WebForecastReport.Interface.MPR
{
    interface IDRService
    {
        List<DailyActivityModel> GetDailyActivities(string user_name, DateTime start_date, DateTime stop_date);
        string AddDailyReport(DailyActivityModel dlr);
        string EditDailyReport(DailyActivityModel dlr);
    }
}
