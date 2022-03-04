using WebForecastReport.Interfaces.MPR;
using WebForecastReport.Models.MPR;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using WebForecastReport.Service;

namespace WebForecastReport.Services.MPR
{
    public class TaskService : ITask
    {
        public List<TaskModel> GetTasks()
        {
            try
            {
                List<TaskModel> tasks = new List<TaskModel>();
                string string_command = string.Format($@"SELECT * FROM Tasks LEFT JOIN Jobs ON Tasks.job_id = Jobs.job_id");
                SqlCommand cmd = new SqlCommand(string_command, ConnectSQL.OpenConnect());
                SqlDataReader dr = cmd.ExecuteReader();
                if (dr.HasRows)
                {
                    while (dr.Read())
                    {
                        TaskModel task = new TaskModel()
                        {
                            job_id = dr["job_id"] != DBNull.Value ? dr["job_id"].ToString() : "",
                            job_name = dr["job_name"] != DBNull.Value ? dr["job_name"].ToString() : "",
                            task_id = dr["task_id"] != DBNull.Value ? dr["task_id"].ToString() : "",
                            task_name = dr["task_name"] != DBNull.Value ? dr["task_name"].ToString() : "",
                            task_description = dr["task_description"] != DBNull.Value ? dr["task_description"].ToString() : "",
                            status = dr["status"] != DBNull.Value ? dr["status"].ToString() : "",
                        };
                        tasks.Add(task);
                    }
                    dr.Close();
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

        public string CreateTask(TaskModel task)
        {
            try
            {
                string string_command = string.Format($@"INSERT INTO Tasks(task_name, job_id) VALUES(@task_name, @job_id)");
                using (SqlCommand cmd = new SqlCommand(string_command, ConnectSQL.OpenConnect()))
                {
                    cmd.CommandType = System.Data.CommandType.Text;
                    cmd.Parameters.AddWithValue("@task_name", task.task_name);
                    cmd.Parameters.AddWithValue("@job_id", task.job_id);
                    cmd.ExecuteNonQuery();
                }
                return "Success";
            }
            finally
            {
                if (ConnectSQL.con.State == System.Data.ConnectionState.Open)
                {
                    ConnectSQL.CloseConnect();
                }
            }
        }

        public string UpdateTask(TaskModel task)
        {
            try
            {
                string string_command = string.Format($@"
                UPDATE TASKS 
                SET
                    task_name = @task_name,
                    job_id = @job_id,
                WHERE task_id = @task_id");
                using (SqlCommand cmd = new SqlCommand(string_command, ConnectSQL.OpenConnect()))
                {
                    cmd.CommandType = System.Data.CommandType.Text;
                    cmd.Parameters.AddWithValue("@task_id", task.task_id);
                    cmd.Parameters.AddWithValue("@task_name", task.task_name);
                    cmd.Parameters.AddWithValue("@job_id", task.job_id);
                    cmd.ExecuteNonQuery();
                }
                return "Success";
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
