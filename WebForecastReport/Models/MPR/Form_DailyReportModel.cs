using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebForecastReport.Models.MPR
{
    public class Form_DailyReportModel
    {
        public string name { get; set; }
        public DateTime start_date { get; set; }
        public DateTime stop_date { get; set; }
        public List<DailyActivityModel> datas { get; set; }
    }
}
