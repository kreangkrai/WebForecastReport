using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebForecastReport.Models;

namespace WebForecastReport.Interface
{
    interface IQuotation
    {
        string GetlastQuotation();
        string Insert(QuotationModel model);
        string InsertQuotation(QuotationModel model);
        string Update(QuotationModel model);
        string Delete(QuotationModel model);
        List<QuotationModel> GetQuotation(string name,string role);
    }
}
