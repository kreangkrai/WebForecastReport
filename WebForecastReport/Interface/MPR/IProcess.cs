using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebForecastReport.Models.MPR;

namespace WebForecastReport.Interface.MPR
{
    interface IProcess
    {
        List<EngProcessModel> GetProcesses();
        int GetLastProcessID();
        string CreateProcess(EngProcessModel process);
        string EditProcess(EngProcessModel process);
        string DeleteProcess(EngProcessModel process);
    }
}
