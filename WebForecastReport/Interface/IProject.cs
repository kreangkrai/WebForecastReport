using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebForecastReport.Models;

namespace WebForecastReport.Interface
{
    interface IProject
    {
        List<ProjectModel> GetProjects(string type_brand);
        List<ProjectModel> GetProjectType();
        List<ProjectModel> GetProjectBrand();
        string Delete(string name, string type_brand);
        string Insert(string name, string type_brand);
        string Update(int id, string name, string type_brand);
    }
}
