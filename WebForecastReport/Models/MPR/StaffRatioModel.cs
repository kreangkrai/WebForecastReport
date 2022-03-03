using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebForecastReport.Models.MPR
{
    public class StaffRatioModel
    {
        string job_id { get; set; }
        string staff_id { get; set; }
        double percentage { get; set; }
    }
}
