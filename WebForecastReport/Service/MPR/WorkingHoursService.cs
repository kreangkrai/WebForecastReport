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
    public class WorkingHoursService : IWorkingHours
    {
        public List<WorkingHoursModel> GetWorkingHours()
        {
            try
            {
                List<WorkingHoursModel> whs = new List<WorkingHoursModel>();
                string string_command = string.Format($@"SELECT 
                                                            [WorkingHours].ind, 
                                                            [WorkingHours].user_id, 
                                                            Users.Name, 
                                                            [WorkingHours].working_date, 
                                                            [WorkingHours].job_id,
                                                            [Jobs].job_name,
                                                            [WorkingHours].task_id,
                                                            [Tasks].task_name,
                                                            [WorkingHours].start_time,
                                                            [WorkingHours].stop_time,
                                                            [WorkingHours].lunch,
                                                            [WorkingHours].dinner,
                                                            [WorkingHours].note
                                                        FROM WorkingHours 
                                                            LEFT JOIN [gps_sale_tracking].[dbo].[Sale_User] Users ON WorkingHours.user_id = Users.Login 
                                                            LEFT JOIN [Jobs] ON WorkingHours.job_id = [Jobs].job_id 
                                                            LEFT JOIN [Tasks] ON WorkingHours.task_id = [Tasks].task_id");
                SqlCommand command = new SqlCommand(string_command, ConnectSQL.OpenConnect());
                SqlDataReader data_reader = command.ExecuteReader();
                if (data_reader.HasRows)
                {
                    while (data_reader.Read())
                    {
                        WorkingHoursModel wh = new WorkingHoursModel()
                        {
                            index = data_reader["ind"] != DBNull.Value ? Convert.ToInt32(data_reader["ind"]) : default(Int32),
                            user_id = data_reader["user_id"] != DBNull.Value ? data_reader["user_id"].ToString() : "",
                            user_name = data_reader["Name"] != DBNull.Value ? data_reader["Name"].ToString() : "",
                            working_date = data_reader["working_date"] != DBNull.Value ? Convert.ToDateTime(data_reader["working_date"]) : default(DateTime),
                            job_id = data_reader["job_id"] != DBNull.Value ? data_reader["job_id"].ToString() : "",
                            job_name = data_reader["job_name"] != DBNull.Value ? data_reader["job_name"].ToString() : "",
                            task_id = data_reader["task_id"] != DBNull.Value ? data_reader["task_id"].ToString() : "",
                            task_name = data_reader["task_name"] != DBNull.Value ? data_reader["task_name"].ToString() : "",
                            start_time = data_reader["start_time"] != DBNull.Value ? TimeSpan.Parse(data_reader["start_time"].ToString()) : default(TimeSpan),
                            stop_time = data_reader["stop_time"] != DBNull.Value ? TimeSpan.Parse(data_reader["stop_time"].ToString()) : default(TimeSpan),
                            lunch = data_reader["lunch"] != DBNull.Value ? Convert.ToBoolean(data_reader["lunch"]) : true,
                            dinner = data_reader["dinner"] != DBNull.Value ? Convert.ToBoolean(data_reader["dinner"]) : true,
                            note = data_reader["note"] != DBNull.Value ? data_reader["note"].ToString() : "",
                        };
                        whs.Add(wh);
                    }
                    data_reader.Close();
                }
                return whs;
            }
            finally
            {
                if (ConnectSQL.con.State == System.Data.ConnectionState.Open)
                {
                    ConnectSQL.CloseConnect();
                }
            }
        }

        public List<WorkingHoursModel> GetWorkingHours(string user_id)
        {
            try
            {
                List<WorkingHoursModel> whs = new List<WorkingHoursModel>();
                string string_command = string.Format($@"
                SELECT 
                    [WorkingHours].ind,
                    [WorkingHours].user_id,
                    [Users].Name,
                    [WorkingHours].working_date,
                    [WorkingHours[.job_id,
                    [Jobs].job_name,
                    [WorkingHours].task_id,
                    [Tasks].task_name,
                    [WorkingHours].start_time,
                    [WorkingHours].stop_time,
                    [WorkingHours].lunch,
                    [WorkingHours].dinner,
                    [WorkingHours].note
                FROM WorkingHours
                    LEFT JOIN [gps_sale_tracking].[dbo].[Sale_User] Users ON WorkingHours.user_id = Users.Login 
                    LEFT JOIN [Jobs] ON [WorkingHours].job_id = [Jobs].job_id
                    LEFT JOIN [Tasks] ON [WorkingHours].task_id = [Tasks].task_id
                WHERE [WorkingHours].user_id = '{user_id}'");
                SqlCommand command = new SqlCommand(string_command, ConnectSQL.OpenConnect());
                SqlDataReader data_reader = command.ExecuteReader();
                if (data_reader.HasRows)
                {
                    while (data_reader.Read())
                    {
                        WorkingHoursModel wh = new WorkingHoursModel()
                        {
                            index = data_reader["ind"] != DBNull.Value ? Convert.ToInt32(data_reader["ind"]) : default(Int32),
                            user_id = data_reader["user_id"] != DBNull.Value ? data_reader["user_id"].ToString() : "",
                            user_name = data_reader["Name"] != DBNull.Value ? data_reader["Name"].ToString() : "",
                            working_date = data_reader["working_date"] != DBNull.Value ? Convert.ToDateTime(data_reader["working_date"]) : default(DateTime),
                            job_id = data_reader["job_id"] != DBNull.Value ? data_reader["job_id"].ToString() : "",
                            job_name = data_reader["job_name"] != DBNull.Value ? data_reader["job_name"].ToString() : "",
                            task_id = data_reader["task_id"] != DBNull.Value ? data_reader["task_id"].ToString() : "",
                            task_name = data_reader["task_name"] != DBNull.Value ? data_reader["task_name"].ToString() : "",
                            start_time = data_reader["start_time"] != DBNull.Value ? TimeSpan.Parse(data_reader["start_time"].ToString()) : default(TimeSpan),
                            stop_time = data_reader["stop_time"] != DBNull.Value ? TimeSpan.Parse(data_reader["stop_time"].ToString()) : default(TimeSpan),
                            lunch = data_reader["lunch"] != DBNull.Value ? Convert.ToBoolean(data_reader["lunch"]) : true,
                            dinner = data_reader["dinner"] != DBNull.Value ? Convert.ToBoolean(data_reader["dinner"]) : true,
                            note = data_reader["note"] != DBNull.Value ? data_reader["note"].ToString() : "",
                        };
                        whs.Add(wh);
                    }
                    data_reader.Close();
                }
                return whs;
            }
            finally
            {
                if (ConnectSQL.con.State == System.Data.ConnectionState.Open)
                {
                    ConnectSQL.CloseConnect();
                }
            }
        }

        public List<WorkingHoursModel> GetWorkingHours(string year, string month, string user)
        {
            try
            {
                List<WorkingHoursModel> whs = new List<WorkingHoursModel>();
                string string_command = string.Format($@"
                SELECT
                    [WorkingHours].ind,
                    [WorkingHours].user_id,
                    [Users].Name,
                    [WorkingHours].working_date,
                    [WorkingHours].job_id,
                    [Jobs].job_name,
                    [WorkingHours].task_id,
                    [Tasks].task_name,
                    [WorkingHours].start_time,
                    [WorkingHours].stop_time,
                    [WorkingHours].lunch,
                    [WorkingHours].dinner,
                    [WorkingHours].note
                FROM WorkingHours
                     LEFT JOIN [gps_sale_tracking].[dbo].[Sale_User] Users ON WorkingHours.user_id = Users.Login 
                    LEFT JOIN [Jobs] ON [WorkingHours].job_id = [Jobs].job_id
                    LEFT JOIN [Tasks] ON [WorkingHours].task_id = [Tasks].task_id
                WHERE [WorkingHours].working_date like '{year}-{month}%' 
                AND [WorkingHours].user_id ='{user}'");
                SqlCommand command = new SqlCommand(string_command, ConnectSQL.OpenConnect());
                SqlDataReader data_reader = command.ExecuteReader();
                if (data_reader.HasRows)
                {
                    while (data_reader.Read())
                    {
                        WorkingHoursModel wh = new WorkingHoursModel()
                        {
                            index = data_reader["ind"] != DBNull.Value ? Convert.ToInt32(data_reader["ind"]) : default(Int32),
                            user_id = data_reader["user_id"] != DBNull.Value ? data_reader["user_id"].ToString() : "",
                            user_name = data_reader["Name"] != DBNull.Value ? data_reader["Name"].ToString() : "",
                            working_date = data_reader["working_date"] != DBNull.Value ? Convert.ToDateTime(data_reader["working_date"]) : default(DateTime),
                            job_id = data_reader["job_id"] != DBNull.Value ? data_reader["job_id"].ToString() : "",
                            job_name = data_reader["job_name"] != DBNull.Value ? data_reader["job_name"].ToString() : "",
                            task_id = data_reader["task_id"] != DBNull.Value ? data_reader["task_id"].ToString() : "",
                            task_name = data_reader["task_name"] != DBNull.Value ? data_reader["task_name"].ToString() : "",
                            start_time = data_reader["start_time"] != DBNull.Value ? TimeSpan.Parse(data_reader["start_time"].ToString()) : default(TimeSpan),
                            stop_time = data_reader["stop_time"] != DBNull.Value ? TimeSpan.Parse(data_reader["stop_time"].ToString()) : default(TimeSpan),
                            lunch = data_reader["lunch"] != DBNull.Value ? Convert.ToBoolean(data_reader["lunch"]) : true,
                            dinner = data_reader["dinner"] != DBNull.Value ? Convert.ToBoolean(data_reader["dinner"]) : true,
                            note = data_reader["note"] != DBNull.Value ? data_reader["note"].ToString() : "",
                        };
                        whs.Add(wh);
                    }
                    data_reader.Close();
                }
                return whs;
            }
            finally
            {
                if (ConnectSQL.con.State == System.Data.ConnectionState.Open)
                {
                    ConnectSQL.CloseConnect();
                }
            }
        }

        public List<WorkingHoursModel> GetWorkingHours(string user, DateTime working_date)
        {
            try
            {
                List<WorkingHoursModel> whs = new List<WorkingHoursModel>();
                string string_command = string.Format($@"
                SELECT
                    ind,
                    [WorkingHours].user_id,
                    [Users].Name,
                    [WorkingHours].working_date,
                    [WorkingHours].job_id,
                    [Jobs].job_name,
                    [WorkingHours].task_id,
                    [Tasks].task_name,
                    [WorkingHours].start_time,
                    [WorkingHours].stop_time,
                    [WorkingHours].lunch,
                    [WorkingHours].dinner,
                    [WorkingHours].note
                FROM WorkingHours
                    LEFT JOIN [gps_sale_tracking].[dbo].[Sale_User] Users ON WorkingHours.user_id = Users.Login 
                    LEFT JOIN [Jobs] ON [WorkingHours].job_id = [Jobs].job_id
                    LEFT JOIN [Tasks] ON [WorkingHours].task_id = [Tasks].task_id
                WHERE [WorkingHours].user_id = '{user}'
                AND [WorkingHours].working_date LIKE '{working_date.ToString("yyyy-MM-dd")}'");
                SqlCommand command = new SqlCommand(string_command, ConnectSQL.OpenConnect());
                SqlDataReader data_reader = command.ExecuteReader();
                if (data_reader.HasRows)
                {
                    while (data_reader.Read())
                    {
                        WorkingHoursModel wh = new WorkingHoursModel()
                        {
                            index = data_reader["ind"] != DBNull.Value ? Convert.ToInt32(data_reader["ind"]) : default(Int32),
                            user_id = data_reader["user_id"] != DBNull.Value ? data_reader["user_id"].ToString() : "",
                            user_name = data_reader["Name"] != DBNull.Value ? data_reader["Name"].ToString() : "",
                            working_date = data_reader["working_date"] != DBNull.Value ? Convert.ToDateTime(data_reader["working_date"]) : default(DateTime),
                            job_id = data_reader["job_id"] != DBNull.Value ? data_reader["job_id"].ToString() : "",
                            job_name = data_reader["job_name"] != DBNull.Value ? data_reader["job_name"].ToString() : "",
                            task_id = data_reader["task_id"] != DBNull.Value ? data_reader["task_id"].ToString() : "",
                            task_name = data_reader["task_name"] != DBNull.Value ? data_reader["task_name"].ToString() : "",
                            start_time = data_reader["start_time"] != DBNull.Value ? TimeSpan.Parse(data_reader["start_time"].ToString()) : default(TimeSpan),
                            stop_time = data_reader["stop_time"] != DBNull.Value ? TimeSpan.Parse(data_reader["stop_time"].ToString()) : default(TimeSpan),
                            lunch = data_reader["lunch"] != DBNull.Value ? Convert.ToBoolean(data_reader["lunch"]) : true,
                            dinner = data_reader["dinner"] != DBNull.Value ? Convert.ToBoolean(data_reader["dinner"]) : true,
                            note = data_reader["note"] != DBNull.Value ? data_reader["note"].ToString() : "",
                        };
                        whs.Add(wh);
                    }
                    data_reader.Close();
                }
                return whs;
            }
            finally
            {
                if (ConnectSQL.con.State == System.Data.ConnectionState.Open)
                {
                    ConnectSQL.CloseConnect();
                }
            }
        }

        public string AddWorkingHours(WorkingHoursModel wh)
        {
            try
            {
                string string_command = string.Format($@"
                INSERT INTO WorkingHours(
                    user_id, working_date, job_id, task_id, start_time, stop_time, lunch, dinner, note)
                VALUES (
                    @user_id, @working_date, @job_id, @task_id, @start_time, @stop_time, @lunch, @dinner, @note)");
                using (SqlCommand command = new SqlCommand(string_command, ConnectSQL.OpenConnect()))
                {
                    command.CommandType = System.Data.CommandType.Text;
                    command.Parameters.AddWithValue("@user_id", wh.user_id);
                    command.Parameters.AddWithValue("@working_date", wh.working_date);
                    command.Parameters.AddWithValue("@job_id", wh.job_id);
                    command.Parameters.AddWithValue("@task_id", wh.task_id);
                    command.Parameters.AddWithValue("@start_time", wh.start_time);
                    command.Parameters.AddWithValue("@stop_time", wh.stop_time);
                    command.Parameters.AddWithValue("@lunch", wh.lunch);
                    command.Parameters.AddWithValue("@dinner", wh.dinner);
                    command.Parameters.AddWithValue("@note", wh.note);
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

        public string UpdateWorkingHours(WorkingHoursModel wh)
        {
            try
            {
                string string_command = string.Format($@"
                UPDATE WorkingHours 
                SET
                    user_id = @user_id,
                    working_date = @working_date,
                    job_id = @job_id,
                    task_id = @task_id,
                    start_time = @start_time,
                    stop_time = @stop_time,
                    lunch = @lunch,
                    dinner = @dinner,
                    note = @note
                WHERE ind = @ind");
                using (SqlCommand command = new SqlCommand(string_command, ConnectSQL.OpenConnect()))
                {
                    command.CommandType = System.Data.CommandType.Text;
                    command.Parameters.AddWithValue("@user_id", wh.user_id);
                    command.Parameters.AddWithValue("@working_date", wh.working_date);
                    command.Parameters.AddWithValue("@job_id", wh.job_id);
                    command.Parameters.AddWithValue("@task_id", wh.task_id);
                    command.Parameters.AddWithValue("@start_time", wh.start_time);
                    command.Parameters.AddWithValue("@stop_time", wh.stop_time);
                    command.Parameters.AddWithValue("@lunch", wh.lunch);
                    command.Parameters.AddWithValue("@dinner", wh.dinner);
                    command.Parameters.AddWithValue("@note", wh.note);
                    command.Parameters.AddWithValue("@ind", wh.index);
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
