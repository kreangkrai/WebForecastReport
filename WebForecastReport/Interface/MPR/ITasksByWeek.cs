using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebForecastReport.Models.MPR;

namespace WebForecastReport.Interface.MPR
{
    interface ITasksByWeek
    {
        List<TasksByWeekModel> GetTasksByWeek(string year, string week);
    }
}
