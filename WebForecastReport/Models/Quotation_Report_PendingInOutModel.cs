using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebForecastReport.Models
{
    public class Quotation_Report_PendingInOutModel
    {
        public string department { get; set; }
        public string sale_name { get; set; }
        public string product_type { get; set; }
        public string type { get; set; }
        public string brand { get; set; }
        public string pending_in { get; set; }
        public string pending_out { get; set; }
        public string pending { get; set; }
    }
}
