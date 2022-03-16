using WebForecastReport.Models.MPR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebForecastReport.Interfaces.MPR
{
    interface IEngUser
    {
        bool CheckAllowEditable(string user_id);
        List<EngUserModel> GetUsers();
        List<EngUserModel> GetEngineerUsers();
        EngUserModel GetEngineerUser(string user_id);
        string CreateEngineerUser(EngUserModel engineer);
        string UpdateEngineerUser(EngUserModel engineer);
    }
}
