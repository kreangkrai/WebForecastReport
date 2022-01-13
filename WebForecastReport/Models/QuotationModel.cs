﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebForecastReport.Models
{
    public class QuotationModel
    {
        public string quotation_no { get; set; }
        public DateTime date { get; set; }
        public string customer { get; set; }
        public string enduser { get; set; }
        public string project_name { get; set; }
        public string site_location { get; set; }
        public string product_type { get; set; }
        public string part_no { get; set; }
        public string spec { get; set; }
        public string quantity { get; set; }
        public string supplier_quotation_no { get; set; }
        public string total_value { get; set; }
        public string unit { get; set; }
        public DateTime expected_order_date { get; set; }
        public DateTime required_onsite_date { get; set; }
        public string proposer { get; set; }
        public DateTime expected_date { get; set; }
        public string status { get; set; }
        public string stages { get; set; }
        public string how_to_support { get; set; }
        public string competitor { get; set; }
        public string competitor_description { get; set; }
        public string competitor_price { get; set; }
        public string sale_name { get; set; }
        public string detail { get; set; }
    }
}
