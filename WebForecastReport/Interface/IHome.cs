using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebForecastReport.Models;

namespace WebForecastReport.Interface
{
    interface IHome
    {
        List<Home_DataModel> getDataByIndividual(string year, string name);
        List<Home_DataModel> getDataByDepartment(string year, string department);
        List<Home_StagesModel> getDataStagesByIndividual(string year, string name);
        List<Home_StagesModel> getDataStagesByDepartment(string year, string department);
        Home_DayModel getDataDay(string year, string name);
        List<Home_DayModel> getDataDayByDepartment(string year, string department);
        List<Home_Stages_DayModel> getDataQuotationMoreDay(string year, string sale_name, string day);

        List<PerformanceModel> getPerformance(string year, string department);

        List<PerformanceModel> getPerformanceStack(string year, string department);

        List<HittingRateModel> GetHittingRateByDepartment(string year,string department);
        List<HittingRateModel> GetHittingRateByName(string year,string name);

        TargetIndividual GetTargetIndividual(string year, string name);
        TargetDepartment GetTargetDepartment(string year, string department);
        List<SubQuotationModel> GetDataSubQuotationIndividual(string year, string name, string type);
        List<SubQuotationModel> GetDataSubQuotationDepartment(string year, string department, string type);
        QuotationModel GetViewQuotationByNo(string quotation);
        List<QuotationModel> GetQuotationByPieIndividual(string year, string name, string type,string data);
        List<QuotationModel> GetQuotationByPieDepartment(string year, string department, string type,string data);
        List<QuotationModel> GetQuotationByBarIndividual(string year, string name, string title,string type, string data);
        List<QuotationModel> GetQuotationByBarDepartment(string year, string department, string title, string type, string data);
    }
}
