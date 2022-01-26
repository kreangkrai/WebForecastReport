using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebForecastReport.Models
{
    public class ProposalModel
    {
        public string proposal_created_by { get; set; }
        public string proposal_department { get; set; }
        public string request_date { get; set; }
        public string proposal_status { get; set; }
        public string proposal_revision { get; set; }
        public string proposal_cost { get; set; }
        public string proposal_quoted_price { get; set; }
        public string gp { get; set; }
        public string finish_date { get; set; }
        public string engineering_request { get; set; }
        public string ppc_request { get; set; }
        public string person_in_charge { get; set; }

        public QuotationModel quotation { get; set; }

    }
}
