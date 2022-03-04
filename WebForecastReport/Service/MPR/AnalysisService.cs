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
            List<TaskTotalHoursModel> tasks = new List<TaskTotalHoursModel>();
            try
            {
                string string_command = string.Format($@"
                    SELECT
                        WorkingHours.job_id,
                        Jobs.job_name,
                        WorkingHours.task_id,
                        Tasks.task_name,
                        SUM(DATEDIFF(HOUR, WorkingHours.start_time, WorkingHours.stop_time)) as total_hours
                    FROM WorkingHours
                        LEFT JOIN Jobs ON WorkingHours.job_id = Jobs.job_id
                        LEFT JOIN Tasks ON WorkingHours.task_id = Tasks.task_id
                    WHERE WorkingHours.job_id = '{job_id}'
                    GROUP BY WorkingHours.job_id, Jobs.job_name, WorkingHours.task_id, Tasks.task_name");
                SqlCommand cmd = new SqlCommand(string_command, ConnectSQL.OpenConnect());
                if (ConnectSQL.con.State != System.Data.ConnectionState.Open)
                {
                    ConnectSQL.CloseConnect();
                    ConnectSQL.OpenConnect();
                }
                SqlDataReader dr = cmd.ExecuteReader();
                if (dr.HasRows)
                {
                    while (dr.Read())
                    {
                        TaskTotalHoursModel task = new TaskTotalHoursModel()
                        {
                            job_id = dr["job_id"] != DBNull.Value ? dr["job_id"].ToString() : "",
                            job_name = dr["job_name"] != DBNull.Value ? dr["job_name"].ToString() : "",
                            task_id = dr["task_id"] != DBNull.Value ? dr["task_id"].ToString() : "",
                            task_name = dr["task_name"] != DBNull.Value ? dr["task_name"].ToString() : "",
                            hours = dr["total_hours"] != DBNull.Value ? Convert.ToInt32(dr["total_hours"].ToString()) : 0,
                        };
                        tasks.Add(task);
                    }
                    dr.Close();
                }
            }
            finally
            {
                if (ConnectSQL.con.State == System.Data.ConnectionState.Open)
                {
                    ConnectSQL.CloseConnect();
                }
            }
            return tasks;
        }

        public List<JobInvolveModel> GetPercentsInvolve(string job_id)
        {
            List<JobInvolveModel> invs = new List<JobInvolveModel>();
            try
            {
                string string_command = string.Format($@"
                    SELECT
                        WorkingHours.job_id,
                        Jobs.job_name,
                        WorkingHours.user_id,
                        Users.Name,
                        SUM(DATEDIFF(HOUR, WorkingHours.start_time, WorkingHours.stop_time)) as total_hours
                    FROM WorkingHours
                        LEFT JOIN Jobs ON WorkingHours.job_id = Jobs.job_id
                        LEFT JOIN gps_sale_tracking.dbo.Sale_User Users ON WorkingHours.user_id = Users.Login 
                    WHERE WorkingHours.job_id = '{job_id}'
                    GROUP BY WorkingHours.job_id, Jobs.job_name, WorkingHours.user_id, Users.Name");
                SqlCommand cmd = new SqlCommand(string_command, ConnectSQL.OpenConnect());
                if (ConnectSQL.con.State != System.Data.ConnectionState.Open)
                {
                    ConnectSQL.CloseConnect();
                    ConnectSQL.OpenConnect();
                }
                SqlDataReader dr = cmd.ExecuteReader();
                if (dr.HasRows)
                {
                    while (dr.Read())
                    {
                        JobInvolveModel inv = new JobInvolveModel()
                        {
                            job_id = dr["job_id"] != DBNull.Value ? dr["job_id"].ToString() : "",
                            job_name = dr["job_name"] != DBNull.Value ? dr["job_name"].ToString() : "",
                            user_id = dr["user_id"] != DBNull.Value ? dr["user_id"].ToString() : "",
                            user_name = dr["Name"] != DBNull.Value ? dr["Name"].ToString() : "",
                            hours = dr["total_hours"] != DBNull.Value ? Convert.ToInt32(dr["total_hours"].ToString()) : 0,
                        };
                        invs.Add(inv);
                    }
                    dr.Close();
                }
            }
            finally
            {
                if (ConnectSQL.con.State == System.Data.ConnectionState.Open)
                {
                    ConnectSQL.CloseConnect();
                }
            }
            return invs;
        }
    }
}