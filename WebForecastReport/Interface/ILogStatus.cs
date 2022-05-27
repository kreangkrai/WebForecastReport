using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebForecastReport.Models;

namespace WebForecastReport.Interface
{
    interface ILogStatus
    {
        string Insert(Log_StatusModel model);
        List<Log_StatusModel> GetStatusByYear(string year);
    }
}
