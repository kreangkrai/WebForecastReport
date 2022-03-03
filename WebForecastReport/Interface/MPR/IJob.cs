﻿using WebForecastReport.Models.MPR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebForecastReport.Interfaces.MPR
{
    interface IJob
    {
        List<JobModel> GetAllJobs();
        string CreateJob(JobModel job);
        string UpdateJob(JobModel job);
    }
}
