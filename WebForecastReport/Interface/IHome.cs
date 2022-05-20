using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebForecastReport.Models;

namespace WebForecastReport.Interface
{
    interface IHome
    {
        List<Home_DataModel> getData(string year, string name);
        List<Home_StagesModel> getDataStages(string year, string name);
        Home_DayModel getDataDay(string year, string name);
        List<Home_DayModel> getDataDayByDepartment(string year, string department);
        List<Home_Stages_DayModel> getDataQuotationMoreDay(string year, string sale_name, string day);

        List<PerformanceModel> getPerformance(string year, string department);

        List<PerformanceModel> getPerformanceStack(string year, string department);

        List<HittingRateModel> GetHittingRateByDepartment(string department);
        List<HittingRateModel> GetHittingRateByName(string name);
    }
}
