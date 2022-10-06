using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebForecastReport.Models.MPR;

namespace WebForecastReport.Interface.MPR
{
    interface IMilestone
    {
        List<MilestoneModel> GetMilestones();
        int GetLastMilestoneID();
        string CreateMilestone(MilestoneModel ms);
        string EditMilestone(MilestoneModel ms);
    }
}
