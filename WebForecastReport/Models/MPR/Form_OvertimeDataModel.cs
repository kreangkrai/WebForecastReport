using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebForecastReport.Models.MPR
{
    public class Form_OvertimeDataModel
    {
        public string date { get; set; }
        public string day { get; set; }
        public string job { get; set; }
        public string location { get; set; }
        public string task { get; set; }
        public string start_time { get; set; }
        public string stop_time { get; set; }
        public string lunch { get; set; }
        public string dinner { get; set; }
        public string normal { get; set; }
        public string ot1_5 { get; set; }
        public string ot3_0 { get; set; }
        public bool weekend { get; set; }
        public bool holiday { get; set; }
    }
}
