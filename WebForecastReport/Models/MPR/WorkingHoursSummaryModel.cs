using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebForecastReport.Models.MPR
{
    public class WorkingHoursSummaryModel
    {
        public string user_id { get; set; }
        public string user_name { get; set; }
        public string job_id { get; set; }
        public string job_name { get; set; }
        public int normal { get; set; }
        public int ot1_5 { get; set; }
        public int ot3_0 { get; set; }
    }
}
