using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebForecastReport.Models;

namespace WebForecastReport.Interface
{
    interface IService
    {
        List<ServiceModel> GetService(string type_brand);
        List<ServiceModel> GetServiceType();
        List<ServiceModel> GetServiceBrand();
        string Delete(string name, string type_brand);
        string Insert(string name, string type_brand);
        string Update(int id, string name, string type_brand);
    }
}
