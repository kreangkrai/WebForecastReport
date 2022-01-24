using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebForecastReport.Models;

namespace WebForecastReport.Interface
{
    interface IProduct
    {
        List<ProductModel> GetProducts();
        string Insert(string name);
        string Update(int id,string name);
        string Delete(string name);
    }
}
