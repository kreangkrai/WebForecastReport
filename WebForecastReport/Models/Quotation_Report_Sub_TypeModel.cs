using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebForecastReport.Models
{
    public class Quotation_Report_Sub_TypeModel
    {
        public string department { get; set; }
        public string sale_name { get; set; }
        public string product_type { get; set; }
        public string type_won_cnt { get; set; }
        public string type_won_mb { get; set; }
        public string type_lost_cnt { get; set; }
        public string type_lost_mb { get; set; }
        public string type_nogo_cnt { get; set; }
        public string type_nogo_mb { get; set; }
        public string type_pending_cnt { get; set; }
        public string type_pending_mb { get; set; }

    }
}
