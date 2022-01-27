using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebForecastReport.Models;

namespace WebForecastReport.Interface
{
    interface ILog_Expected
    {
        List<Log_ExpectedModel> getLogs();
        List<Log_ExpectedModel> getLogByName(string name);
        string Insert(Log_ExpectedModel model);
    }
}
