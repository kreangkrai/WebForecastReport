using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace WebForecastReport.Interface
{
    interface IExport
    {
        Stream ExportQuotation(FileInfo path, string role, string name, string department);
        Stream ExportQuotation_Report_Department(FileInfo path, string department, string month_first, string month_last);
        Stream ExportQuotation_Report_Quarter(FileInfo path, string department, string year);
    }
}
