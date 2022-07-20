using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebForecastReport.Models.MPR
{
    public class JobWeeklyWorkingHoursModel
    {
        [JsonProperty("job_id")]
        public string job_id { get; set; }

        [JsonProperty("job_name")]
        public string job_name { get; set; }

        [JsonProperty("quotation_no")]
        public string quotation_no { get; set; }

        [JsonProperty("customer")]
        public string customer { get; set; }

        [JsonProperty("year")]
        public int year { get; set; }

        [JsonProperty("week")]
        public int week { get; set; }

        [JsonProperty("hours")]
        public int hours { get; set; }
    }
}
