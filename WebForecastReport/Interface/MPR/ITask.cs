using WebForecastReport.Models.MPR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebForecastReport.Interfaces.MPR
{
    interface ITask
    {
        List<TaskModel> GetTasks();
        string CreateTask(TaskModel task);
        string UpdateTask(TaskModel task);

    }
}
