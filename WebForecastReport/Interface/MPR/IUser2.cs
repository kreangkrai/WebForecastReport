using WebForecastReport.Models.MPR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebForecastReport.Interfaces.MPR
{
    interface IUser2
    {
        List<UserModel2> GetUsers();
        //string CreateUser(UserModel2 user);
        //string UpdateUser(UserModel2 user);
    }
}
