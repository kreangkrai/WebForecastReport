using WebForecastReport.Models.MPR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebForecastReport.Interfaces.MPR
{
    interface ITask
    {
        List<TaskModel> GetAllTasks();
        List<TaskModel> GetOfficeTasks();
        List<TaskModel> GetSiteTasks();
        string CreateTask(TaskModel task);
        string UpdateTask(TaskModel task);

    }
}
