using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebForecastReport.Models;

namespace WebForecastReport.Interface
{
    interface IAccessory
    {
        List<SaleModel> getSale();
        List<UserModel> getAllUser();
    }
}
