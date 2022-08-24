using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebForecastReport.Models.MPR
{
    public class EngineerScoreModel
    {
        public string job_id { get; set; }
        public string job_name { get; set; }
        public string job_status { get; set; }
        public int cost { get; set; }
        public double md_rate { get; set; }
        public double pd_rate { get; set; }
        public double factor { get; set; }
        public int total_manpower { get; set; }
        public double cost_per_tmp { get; set; }
        public int manpower { get; set; }
        public double manpower_per_tmp { get; set; }
        public double score { get; set; }
    }
}
