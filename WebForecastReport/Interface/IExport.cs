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
    }
}
