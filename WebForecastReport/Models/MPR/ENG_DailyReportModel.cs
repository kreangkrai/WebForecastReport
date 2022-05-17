using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebForecastReport.Models.MPR
{
    public class ENG_DailyReportModel
    {
        public int index { get; set; }
        public DateTime date { get; set; }
        public TimeSpan start_time { get; set; }
        public TimeSpan stop_time { get; set; }
        public string job_id { get; set; }
        public string job_name { get; set; }
        public string user_id { get; set; }
        public string user_name { get; set; }
        public string activity { get; set; }
        public string problem { get; set; }
        public string solution { get; set; }
        public string tomorrow_plan { get; set; }
        public string customer { get; set; }
        public string status { get; set; }
    }
}
