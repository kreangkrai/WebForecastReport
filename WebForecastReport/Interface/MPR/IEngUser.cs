using WebForecastReport.Models.MPR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebForecastReport.Interfaces.MPR
{
    interface IEngUser
    {
        List<EngUserModel> GetUsers();
        List<EngUserModel> GetEngineerUsers();
        string CreateEngineerUser(EngUserModel engineer);
        string UpdateEngineerUser(EngUserModel engineer);
    }
}
