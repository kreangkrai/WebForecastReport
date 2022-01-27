using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebForecastReport.Models;

namespace WebForecastReport.Interface
{
    interface IProject
    {
        List<ProjectModel> getProjects();
        string Delete(string name);
        string Insert(string name);
        string Update(int id, string name);
    }
}
