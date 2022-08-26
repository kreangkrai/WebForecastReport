using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebForecastReport.Models.MPR;

namespace WebForecastReport.Interface.MPR
{
    interface ISystem
    {
        List<EngSystemModel> GetSystems();
        string CreateSystem(EngSystemModel system);
        string EditSystem(EngSystemModel system);
        string DeleteSystem(EngSystemModel system);
    }
}
