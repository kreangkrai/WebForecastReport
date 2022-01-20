using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebForecastReport.Models;

namespace WebForecastReport.Interface
{
    interface IUser
    {
        List<UserManagementModel> GetUsers();
        string update(string name, string role);
        string insert(string name);
    }
}
