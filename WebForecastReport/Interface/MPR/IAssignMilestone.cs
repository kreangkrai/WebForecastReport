using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebForecastReport.Models.MPR;

namespace WebForecastReport.Interface.MPR
{
    interface IAssignMilestone
    {
        List<AssignMilestoneModel> GetAssignedEngineers();
        List<AssignMilestoneModel> GetEngineerAssignedJobs(string engId);
        List<AssignMilestoneModel> GetJobAssignedEngineers(string jobId);
        string AddEngineer(AssignMilestoneModel asgEng);
        string EditEngineer(AssignMilestoneModel asgEng);
        string DeleteEngineer(AssignMilestoneModel asgEng);
    }
}
