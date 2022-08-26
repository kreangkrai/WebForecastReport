using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebForecastReport.Models.MPR;

namespace WebForecastReport.Interface.MPR
{
    interface ISkill
    {
        List<EngSkillModel> GetSkills();
        string CreateSkill(EngSkillModel skill);
        string EditSkill(EngSkillModel skill);
        string DeleteSkill(EngSkillModel skill);
    }
}
