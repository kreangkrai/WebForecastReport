using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebForecastReport.Models.MPR
{
    public class JobResponsibleModel
    {
        public string job_id { get; set; }
        public string job_name { get; set; }
        public string user_id { get; set; }
        public string user_name { get; set; }
        public string department { get; set; }
        public string role { get; set; }
        public string assign_by { get; set; }
        public DateTime assign_date { get; set; }
    }
}
