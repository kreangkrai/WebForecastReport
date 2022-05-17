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
        public List<WorkingHoursModel> working_hours { get; set; }
        public string approval { get; set; }
        public string inspector { get; set; }
        public string approver { get; set; }
    }
}
