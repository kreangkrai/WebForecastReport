using WebForecastReport.Models.MPR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebForecastReport.Interfaces.MPR
{
    interface IAnalysis
    {
        List<TaskRatioModel> GetTaskRatio(string job_id);
        List<TaskDistributionModel> GetTaskDistribution(string job_id);
        List<ManpowerRatioModel> GetManpowerRatio(string job_id);
        List<ManpowerDistributionModel> GetManpowerDistribution(string job_id);
    }
}
