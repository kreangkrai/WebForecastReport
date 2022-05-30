using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebForecastReport.Models.MPR
{
    public class TaskDistributionModel
    {
        public string job_id { get; set; }
        public string job_name { get; set; }
        public string task_id { get; set; }
        public string task_name { get; set; }
        public double hours { get; set; }
    }
}
