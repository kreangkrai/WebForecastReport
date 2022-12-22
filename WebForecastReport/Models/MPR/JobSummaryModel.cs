using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebForecastReport.Models.MPR
{
    public class JobSummaryModel
    {
        public string jobId { get; set; }
        public string jobName { get; set; }
        public string customer { get; set; }
        public int cost { get; set; }
        public double factor { get; set; }
        public int totalManhour { get; set; }
        public string status { get; set; }
        public int remainingCost { get; set; }
    }
}
