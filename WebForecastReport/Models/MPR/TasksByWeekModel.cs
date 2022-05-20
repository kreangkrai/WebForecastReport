using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebForecastReport.Models.MPR
{
    public class TasksByWeekModel
    {
        public string user_id { get; set; }
        public string user_name { get; set; }
        public int week_number { get; set; }
        public DateTime working_date { get; set; }
        public string job_id { get; set; }
        public string job_name { get; set; }
        public string customer { get; set; }
        public string task_id { get; set; }
        public string task_name { get; set; }
        public TimeSpan start_time { get; set; }
        public TimeSpan stop_time { get; set; }
        public bool lunch { get; set; }
        public bool dinner { get; set; }
    }
}
