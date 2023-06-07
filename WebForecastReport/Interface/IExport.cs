using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using WebForecastReport.Models;

namespace WebForecastReport.Interface
{
    interface IExport
    {
        Stream ExportQuotation(FileInfo path, string role, string name, string department);
        Stream ExportQuotation_Report_Department(FileInfo path, string department, string month_first, string month_last);
        Stream ExportQuotation_Report_Quarter(FileInfo path, string department, string year, string stages);
        Stream ExportQuotation_Report_PendingInOut(FileInfo path, string department, string month_first, string month_last);
        Stream ExportQuotation_Report_Year(FileInfo path, string department, string year);
        Stream ExportLogStatus(FileInfo path, string year);
        Stream ExportLogStages(FileInfo path, string year);
        Stream ExportQuotation_Report_Chart_Individual(FileInfo path, List<Home_DataModel> temp_data,TargetIndividual target);
        Stream ExportQuotation_Report_Chart_Department(FileInfo path, List<Home_DataModel> temp_data, TargetDepartment target);
    }
}
