using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebForecastReport.Models;

namespace WebForecastReport.Interface
{
    interface IQuotation_Report
    {
        List<Quotation_Report_DepartmentModel> GetReportDepartment(string department, string month);
    }
}
