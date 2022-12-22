using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebForecastReport.Models.MPR
{
    public class EngineerIdleTimeModel
    {
        public string userName { get; set; }
        public int workingHours { get; set; }
        public int idle { get; set; }
        public int normal { get; set; }
        public int overtime { get; set; }
    }
}
