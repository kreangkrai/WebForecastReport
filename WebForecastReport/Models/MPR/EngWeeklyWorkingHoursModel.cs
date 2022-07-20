using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebForecastReport.Models.MPR
{
    public class EngWeeklyWorkingHoursModel
    {
        [JsonProperty("user_id")]
        public string user_id { get; set; }

        [JsonProperty("user_name")]
        public string user_name { get; set; }

        [JsonProperty("year")]
        public int year { get; set; }

        [JsonProperty("week")]
        public int week { get; set; }

        [JsonProperty("hours")]
        public int hours { get; set; }
    }
}
