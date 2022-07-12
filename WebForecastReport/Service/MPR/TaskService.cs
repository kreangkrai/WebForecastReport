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
        public List<TaskModel> GetAllTasks()
        {
            List<TaskModel> tasks = new List<TaskModel>();
            try
            {
                string string_command = string.Format($"SELECT * FROM Tasks");
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
                        TaskModel task = new TaskModel()
                        {
                            task_id = dr["task_id"] != DBNull.Value ? dr["task_id"].ToString() : "",
                            task_name = dr["task_name"] != DBNull.Value ? dr["task_name"].ToString() : "",
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

        public List<TaskModel> GetOfficeTasks()
        {
            List<TaskModel> tasks = new List<TaskModel>();
            try
            {
                string string_command = string.Format($"SELECT * FROM Tasks WHERE Tasks.task_id LIKE 'O%'");
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
                        TaskModel task = new TaskModel()
                        {
                            task_id = dr["task_id"] != DBNull.Value ? dr["task_id"].ToString() : "",
                            task_name = dr["task_name"] != DBNull.Value ? dr["task_name"].ToString() : "",
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

        public List<TaskModel> GetSiteTasks()
        {
            List<TaskModel> tasks = new List<TaskModel>();
            try
            {
                string string_command = string.Format($"SELECT * FROM Tasks WHERE Tasks.task_id LIKE 'S%'");
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
                        TaskModel task = new TaskModel()
                        {
                            task_id = dr["task_id"] != DBNull.Value ? dr["task_id"].ToString() : "",
                            task_name = dr["task_name"] != DBNull.Value ? dr["task_name"].ToString() : "",
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

        public List<TaskModel> GetTravelTasks()
        {
            List<TaskModel> tasks = new List<TaskModel>();
            try
            {
                string string_command = string.Format($"SELECT * FROM Tasks WHERE Tasks.task_id LIKE 'T%'");
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
                        TaskModel task = new TaskModel()
                        {
                            task_id = dr["task_id"] != DBNull.Value ? dr["task_id"].ToString() : "",
                            task_name = dr["task_name"] != DBNull.Value ? dr["task_name"].ToString() : "",
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

        public string CreateTask(TaskModel task)
        {
            try
            {
                string string_command = string.Format($@"INSERT INTO Tasks(task_id, task_name) VALUES(@task_id, @task_name)");
                using (SqlCommand cmd = new SqlCommand(string_command, ConnectSQL.OpenConnect()))
                {
                    cmd.CommandType = System.Data.CommandType.Text;
                    cmd.Parameters.AddWithValue("@task_id", task.task_id);
                    cmd.Parameters.AddWithValue("@task_name", task.task_name);
                    if (ConnectSQL.con.State != System.Data.ConnectionState.Open)
                    {
                        ConnectSQL.CloseConnect();
                        ConnectSQL.OpenConnect();
                    }
                    cmd.ExecuteNonQuery();
                }
            }
            finally
            {
                if (ConnectSQL.con.State == System.Data.ConnectionState.Open)
                {
                    ConnectSQL.CloseConnect();
                }
            }
            return "Success";
        }

        public string UpdateTask(TaskModel task)
        {
            try
            {
                string string_command = string.Format($@"
                UPDATE TASKS 
                SET
                    task_name = @task_name
                WHERE task_id = @task_id");
                using (SqlCommand cmd = new SqlCommand(string_command,ConnectSQL.OpenConnect()))
                {
                    cmd.CommandType = System.Data.CommandType.Text;
                    cmd.Parameters.AddWithValue("@task_id", task.task_id);
                    cmd.Parameters.AddWithValue("@task_name", task.task_name);
                    if (ConnectSQL.con.State != System.Data.ConnectionState.Open)
                    {
                        ConnectSQL.CloseConnect();
                        ConnectSQL.OpenConnect();
                    }
                    cmd.ExecuteNonQuery();
                }
            }
            finally
            {
                if (ConnectSQL.con.State == System.Data.ConnectionState.Open)
                {
                    ConnectSQL.CloseConnect();
                }
            }
            return "Success";
        }
    }
}
