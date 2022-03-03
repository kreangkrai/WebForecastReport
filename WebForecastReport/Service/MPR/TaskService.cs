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
                SqlCommand command = new SqlCommand(string_command, ConnectSQL.OpenConnect());
                SqlDataReader data_reader = command.ExecuteReader();
                if (data_reader.HasRows)
                {
                    while (data_reader.Read())
                    {
                        TaskModel task = new TaskModel()
                        {
                            job_id = data_reader["job_id"] != DBNull.Value ? data_reader["job_id"].ToString() : "",
                            job_name = data_reader["job_name"] != DBNull.Value ? data_reader["job_name"].ToString() : "",
                            task_id = data_reader["task_id"] != DBNull.Value ? data_reader["task_id"].ToString() : "",
                            task_name = data_reader["task_name"] != DBNull.Value ? data_reader["task_name"].ToString() : "",
                            task_description = data_reader["task_description"] != DBNull.Value ? data_reader["task_description"].ToString() : "",
                            status = data_reader["status"] != DBNull.Value ? data_reader["status"].ToString() : "",
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

        public string CreateTask(TaskModel task)
        {
            try
            {
                string string_command = string.Format($@"INSERT INTO Tasks(task_name, job_id) VALUES(@task_name, @job_id)");
                using (SqlCommand command = new SqlCommand(string_command, ConnectSQL.OpenConnect()))
                {
                    command.CommandType = System.Data.CommandType.Text;
                    command.Parameters.AddWithValue("@task_name", task.task_name);
                    command.Parameters.AddWithValue("@job_id", task.job_id);
                    command.ExecuteNonQuery();
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
                using (SqlCommand command = new SqlCommand(string_command, ConnectSQL.OpenConnect()))
                {
                    command.CommandType = System.Data.CommandType.Text;
                    command.Parameters.AddWithValue("@task_id", task.task_id);
                    command.Parameters.AddWithValue("@task_name", task.task_name);
                    command.Parameters.AddWithValue("@job_id", task.job_id);
                    command.ExecuteNonQuery();
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
