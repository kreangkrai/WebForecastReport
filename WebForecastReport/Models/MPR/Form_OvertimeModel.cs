using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebForecastReport.Models.MPR
{
    public class Form_OvertimeModel
    {
        public string employee_name { get; set; }
        public string department { get; set; }
        public string phone_number { get; set; }
        public string normal_start_time { get; set; }
        public string month { get; set; }
        public List<Form_OvertimeDataModel> datas { get; set; }
        public List<WorkingHoursModel> summary { get; set; }
        public string total_working_hours { get; set; }
        public string total_normal { get; set; }
        public string total_ot1_5 { get; set; }
        public string total_ot3_0 { get; set; }
        public double hours_normal { get; set; }
        public double hours_1_5 { get; set; }
        public double hours_3_0 { get; set; }
    }
}
