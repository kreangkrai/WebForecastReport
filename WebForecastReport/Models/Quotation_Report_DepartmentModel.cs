using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebForecastReport.Models
{
    public class Quotation_Report_DepartmentModel
    {
        public string department { get; set; }
        public string sale { get; set; }
        public string quo_mb { get; set; }
        public string quo_cnt { get; set; }
        public string product_cnt { get; set; }
        public string product_mb { get; set; }
        public string project_cnt { get; set; }
        public string project_mb { get; set; }
        public string service_cnt { get; set; }
        public string service_mb { get; set; }
        public string won_quo_cnt { get; set; }
        public string won_mb { get; set; }
        public string loss_quo_cnt { get; set; }
        public string loss_mb { get; set; }
        public string nogo_quo_cnt { get; set; }
        public string nogo_mb { get; set; }
        public string pending_quo_cnt { get; set; }
        public string pending_mb { get; set; }

        public Quotation_Report_Sub_TypeModel quotation_sub_type { get; set; }
        public Quotation_Report_Sub_StagesModel quotation_sub_stages { get; set; }
        public Quotation_Report_Sub_PendingModel quotation_sub_pending { get; set; }

    }
}
