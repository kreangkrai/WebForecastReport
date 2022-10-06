using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebForecastReport.Models.MPR
{
    public class JobMilestoneModel
    {
        public int no { get; set; }
        public string job_milestone_id { get; set; }
        public string job_id { get; set; }
        public string job_name { get; set; }
        public string customer { get; set; }
        public string milestone_id { get; set; }
        public string milestone_name { get; set; }
        public DateTime start_date { get; set; }
        public DateTime stop_date { get; set; }
    }
}
