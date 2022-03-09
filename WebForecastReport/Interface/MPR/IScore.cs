using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebForecastReport.Models.MPR;

namespace WebForecastReport.Interface.MPR
{
    interface IScore
    {
        List<EngineerScoreModel> GetScores(string user_id);
    }
}
