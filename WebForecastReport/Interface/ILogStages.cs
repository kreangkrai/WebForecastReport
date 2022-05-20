using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebForecastReport.Models;

namespace WebForecastReport.Interface
{
    interface ILogStages
    {
        string Insert(Log_StagesModel model);
        List<Log_StagesModel> GetStages();
        List<Log_StagesModel> GetStagesByName(string name);
    }
}
