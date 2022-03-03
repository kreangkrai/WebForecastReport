using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using WebForecastReport.Service;
using WebForecastReport.Interfaces.MPR;
using WebForecastReport.Models.MPR;

namespace WebForecastReport.Services.MPR
{
    public class AnalysisService : IAnalysis
    {
        public List<TaskTotalHoursModel> GetTasksWorkingHours(string job_id)
        {
            try
            {
                List<TaskTotalHoursModel> tasks = new List<TaskTotalHoursModel>();
                string string_command = string.Format($@"
                    SELECT
                        [WorkingHours].job_id,
                        [Jobs].job_name,
                        [WorkingHours].task_id,
                        [Tasks].task_name,
                        SUM(DATEDIFF(HOUR, [WorkingHours].start_time, [WorkingHours].stop_time)) as total_hours
                    FROM [WorkingHours]
                        LEFT JOIN [Jobs] ON [WorkingHours].job_id = [Jobs].job_id
                        LEFT JOIN [Tasks] ON [WorkingHours].task_id = [Tasks].task_id
                    WHERE [WorkingHours].job_id = '{job_id}'
                    Group by [WorkingHours].job_id, [Jobs].job_name, [WorkingHours].task_id, [Tasks].task_name");
                SqlCommand command = new SqlCommand(string_command, ConnectSQL.OpenConnect());
                SqlDataReader data_reader = command.ExecuteReader();
                if (data_reader.HasRows)
                {
                    while (data_reader.Read())
                    {
                        TaskTotalHoursModel task = new TaskTotalHoursModel()
                        {
                            job_id = data_reader["job_id"] != DBNull.Value ? data_reader["job_id"].ToString() : "",
                            job_name = data_reader["job_name"] != DBNull.Value ? data_reader["job_name"].ToString() : "",
                            task_id = data_reader["task_id"] != DBNull.Value ? data_reader["task_id"].ToString() : "",
                            task_name = data_reader["task_name"] != DBNull.Value ? data_reader["task_name"].ToString() : "",
                            hours = data_reader["total_hours"] != DBNull.Value ? Convert.ToInt32(data_reader["total_hours"].ToString()) : 0,
                        };
                        tasks.Add(task);
                    }
                    data_reader.Close();
                }
                return tasks;
            }
            finally
            {
                if (ConnectSQL.con.State == System.Data.ConnectionState.Open)
                {
                    ConnectSQL.CloseConnect();
                }
            }
        }

        public List<JobInvolveModel> GetPercentsInvolve(string job_id)
        {
            try
            {
                List<JobInvolveModel> invs = new List<JobInvolveModel>();
                string string_command = string.Format($@"
                    SELECT
                        [WorkingHours].job_id,
                        [Jobs].job_name,
                        [WorkingHours].user_id,
                        [Users].Name,
                        SUM(DATEDIFF(HOUR, [WorkingHours].start_time, [WorkingHours].stop_time)) as total_hours
                    FROM [WorkingHours]
                        LEFT JOIN [Jobs] ON [WorkingHours].job_id = [Jobs].job_id
                        LEFT JOIN [gps_sale_tracking].[dbo].[Sale_User] Users ON WorkingHours.user_id = Users.Login 
                    WHERE [WorkingHours].job_id = '{job_id}'
                    Group by [WorkingHours].job_id, [Jobs].job_name, [WorkingHours].user_id, [Users].Name");
                SqlCommand command = new SqlCommand(string_command, ConnectSQL.OpenConnect());
                SqlDataReader data_reader = command.ExecuteReader();
                if (data_reader.HasRows)
                {
                    while (data_reader.Read())
                    {
                        JobInvolveModel inv = new JobInvolveModel()
                        {
                            job_id = data_reader["job_id"] != DBNull.Value ? data_reader["job_id"].ToString() : "",
                            job_name = data_reader["job_name"] != DBNull.Value ? data_reader["job_name"].ToString() : "",
                            user_id = data_reader["user_id"] != DBNull.Value ? data_reader["user_id"].ToString() : "",
                            user_name = data_reader["Name"] != DBNull.Value ? data_reader["Name"].ToString() : "",
                            hours = data_reader["total_hours"] != DBNull.Value ? Convert.ToInt32(data_reader["total_hours"].ToString()) : 0,
                        };
                        invs.Add(inv);
                    }
                    data_reader.Close();
                }
                return invs;
            }
            finally
            {
                if (ConnectSQL.con.State == System.Data.ConnectionState.Open)
                {
                    ConnectSQL.CloseConnect();
                }
            }
        }
    }
}
