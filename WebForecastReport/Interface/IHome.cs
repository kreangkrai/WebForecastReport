using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebForecastReport.Models;

namespace WebForecastReport.Interface
{
    interface IHome
    {
        List<Home_DataModel> getData(string name);
        List<Home_StagesModel> getDataStages(string name);
        Home_DayModel getDataDay(string name);
    }
}
