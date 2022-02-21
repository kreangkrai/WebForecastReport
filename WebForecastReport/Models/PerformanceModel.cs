using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebForecastReport.Models
{
    public class PerformanceModel
    {
        public string department { get; set; }
        public string sale_name { get; set; }
        public string product_target { get; set; }
        public string product_actual { get; set; }
        public string project_target { get; set; }
        public string project_actual { get; set; }
        public string service_target { get; set; }
        public string service_actual { get; set; }

    }
}
