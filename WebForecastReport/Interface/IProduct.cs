using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebForecastReport.Models;

namespace WebForecastReport.Interface
{
    interface IProduct
    {
        List<TypeModel> GetProductType();
        List<TypeModel> GetProductBrand();

        List<TypeModel> GetProducts(string type_brand);
        string Insert(string name, string type_brand);
        string Update(int id, string name, string type_brand);
        string Delete(string name, string type_brand);
    }
}
