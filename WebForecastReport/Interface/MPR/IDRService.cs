using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebForecastReport.Models.MPR;

namespace WebForecastReport.Interface.MPR
{
    interface IDRService
    {
        List<ENG_DailyReportModel> GetDailyReport(string user_id, string month, string job_id);
        string AddDailyReport(ENG_DailyReportModel dlr);
        string EditDailyReport(ENG_DailyReportModel dlr);
    }
}
