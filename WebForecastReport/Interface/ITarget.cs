using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebForecastReport.Models;

namespace WebForecastReport.Interface
{
    interface ITarget
    {
        List<TargetModel> getData();
        string Insert(string year, string department, string name);
        string Update(TargetModel model);
        string Delete(string year, string name);


    }
}
