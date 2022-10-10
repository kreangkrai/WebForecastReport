using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebForecastReport.Models.MPR
{
    public class PlanMandayModel
    {
        public int no { get; set; }
        public string job_milestone_id { get; set; }
        public string job_id { get; set; }
        public string job_name { get; set; }
        public string customer { get; set; }
        public string milestone_id { get; set; }
        public string milestone_name { get; set; }
        public string user_id { get; set; }
        public string user_name { get; set; }
        public string department { get; set; }
        public DateTime date { get; set; }
        public float hours { get; set; }
    }
}
