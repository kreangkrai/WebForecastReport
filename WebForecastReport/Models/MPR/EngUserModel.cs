using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebForecastReport.Models.MPR
{
    public class EngUserModel
    {
        public string user_id { get; set; }
        public string user_name { get; set; }
        public string department { get; set; }
        public bool allow_edit { get; set; }
    }
}
