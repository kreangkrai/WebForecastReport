﻿using WebForecastReport.Models.MPR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebForecastReport.Interfaces.MPR
{
    interface ICalculateWorkingHours
    {
        WorkingHoursModel CalculateOvertime(WorkingHoursModel wh);
    }
}
