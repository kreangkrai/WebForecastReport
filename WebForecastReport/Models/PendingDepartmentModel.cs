using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebForecastReport.Models
{
    public class PendingDepartmentModel
    {
        public string department { get; set; }
        public string product_in { get; set; }
        public string project_in { get; set; }
        public string service_in { get; set; }
        public string product_all { get; set; }
        public string project_all { get; set; }
        public string service_all { get; set; }
    }
}
