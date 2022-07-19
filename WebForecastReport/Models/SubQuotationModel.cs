using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebForecastReport.Models
{
    public class SubQuotationModel
    {
        public string sale_name { get; set; }
        public string department { get; set; }
        public string quotation_no { get; set; }
        //public string customer { get; set; }
        //public string project_name { get; set; }
        public string product_type { get; set; }
        public string quoted_price { get; set; }
        public string status { get; set; }
        public string stages { get; set; }

    }
}
