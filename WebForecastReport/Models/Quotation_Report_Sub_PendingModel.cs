using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebForecastReport.Models
{
    public class Quotation_Report_Sub_PendingModel
    {
        public string department { get; set; }
        public string sale_name { get; set; }
        public string stages { get; set; }
        public string product_type { get; set; }
        public string stages_pending_cnt { get; set; }
        public string stages_pending_mb { get; set; }
    }
}
