using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebForecastReport.Models;

namespace WebForecastReport.Interface
{
    interface IQuotation_Report
    {
        List<Quotation_Report_DepartmentModel> GetReportDepartment(string department, string month_first, string month_last);
        List<Quotation_Report_QuarterModel> GetReportQuarter(string department, string year,string stages);

        List<Quotation_Report_YearModel> GetReportYear(string department, string year);
        List<Quotation_Report_StatusModel> GetReportStatus(string year, string department, string sale);
        List<Quotation_Report_PendingInOutModel> GetReportPendingInOutByDepSale(string department, string month_first, string month_last);
        List<Quotation_Report_PendingInOutModel> GetReportPendingInOutByDepartment(string department, string month_first, string month_last);
    }
}
