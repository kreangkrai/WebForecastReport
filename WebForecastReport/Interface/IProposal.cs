using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebForecastReport.Models;

namespace WebForecastReport.Interface
{
    interface IProposal
    {
        List<ProposalModel> getProposals(string name, string role);
        string Insert(List<QuotationModel> model, List<string> quotations);
        string Update(ProposalModel model);

        List<string> chkQuotation(string name, string role,string department);
        List<string> chkForUpdate(string name, string role, string department);
        string UpdateName(List<string> quotations, string name);
    }
}
