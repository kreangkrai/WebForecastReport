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
            List<WorkingHoursModel> whs = new List<WorkingHoursModel>();
            try
            {
                string string_command = string.Format($@"
                    SELECT
                        WorkingHours.ind,
                        WorkingHours.user_id,
                        Users.Name,
                        WorkingHours.working_date,
                        WorkingHours.week_number,
                        WorkingHours.job_id,
                        Jobs.job_name,
                        WorkingHours.task_id,
                        Tasks.task_name,
                        WorkingHours.start_time,
                        WorkingHours.stop_time,
                        WorkingHours.note
                    FROM WorkingHours
                        LEFT JOIN gps_sale_tracking.dbo.Sale_User Users ON WorkingHours.user_id = Users.Login
                        LEFT JOIN Jobs ON WorkingHours.job_id = Jobs.job_id
                        LEFT JOIN Tasks ON WorkingHours.task_id = Tasks.task_id");
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
                        WorkingHoursModel wh = new WorkingHoursModel()
                        {
                            index = dr["ind"] != DBNull.Value ? Convert.ToInt32(dr["ind"]) : default(Int32),
                            user_id = dr["user_id"] != DBNull.Value ? dr["user_id"].ToString() : "",
                            user_name = dr["Name"] != DBNull.Value ? dr["Name"].ToString() : "",
                            working_date = dr["working_date"] != DBNull.Value ? Convert.ToDateTime(dr["working_date"]) : default(DateTime),
                            week_number = dr["week_number"] != DBNull.Value ? Convert.ToInt32(dr["week_number"]) : default(Int32),
                            job_id = dr["job_id"] != DBNull.Value ? dr["job_id"].ToString() : "",
                            job_name = dr["job_name"] != DBNull.Value ? dr["job_name"].ToString() : "",
                            task_id = dr["task_id"] != DBNull.Value ? dr["task_id"].ToString() : "",
                            task_name = dr["task_name"] != DBNull.Value ? dr["task_name"].ToString() : "",
                            start_time = dr["start_time"] != DBNull.Value ? TimeSpan.Parse(dr["start_time"].ToString()) : default(TimeSpan),
                            stop_time = dr["stop_time"] != DBNull.Value ? TimeSpan.Parse(dr["stop_time"].ToString()) : default(TimeSpan),
                            note = dr["note"] != DBNull.Value ? dr["note"].ToString() : "",
                        };
                        whs.Add(wh);
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
            return whs;
        }

        public List<WorkingHoursModel> GetWorkingHours(string user_id)
        {
            List<WorkingHoursModel> whs = new List<WorkingHoursModel>();
            try
            {
                string string_command = string.Format($@"
                    SELECT 
                        WorkingHours.ind,
                        WorkingHours.user_id,
                        Users.Name,
                        WorkingHours.working_date,
                        WorkingHours.week_number,
                        WorkingHours.job_id,
                        Jobs.job_name,
                        WorkingHours.task_id,
                        Tasks.task_name,
                        WorkingHours.start_time,
                        WorkingHours.stop_time,
                        WorkingHours.note
                    FROM WorkingHours
                        LEFT JOIN gps_sale_tracking.dbo.Sale_User Users ON WorkingHours.user_id = Users.Login 
                        LEFT JOIN Jobs ON WorkingHours.job_id = Jobs.job_id
                        LEFT JOIN Tasks ON WorkingHours.task_id = Tasks.task_id
                    WHERE WorkingHours.user_id = '{user_id}'");
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
                        WorkingHoursModel wh = new WorkingHoursModel()
                        {
                            index = dr["ind"] != DBNull.Value ? Convert.ToInt32(dr["ind"]) : default(Int32),
                            user_id = dr["user_id"] != DBNull.Value ? dr["user_id"].ToString() : "",
                            user_name = dr["Name"] != DBNull.Value ? dr["Name"].ToString() : "",
                            working_date = dr["working_date"] != DBNull.Value ? Convert.ToDateTime(dr["working_date"]) : default(DateTime),
                            week_number = dr["week_number"] != DBNull.Value ? Convert.ToInt32(dr["week_number"]) : default(Int32),
                            job_id = dr["job_id"] != DBNull.Value ? dr["job_id"].ToString() : "",
                            job_name = dr["job_name"] != DBNull.Value ? dr["job_name"].ToString() : "",
                            task_id = dr["task_id"] != DBNull.Value ? dr["task_id"].ToString() : "",
                            task_name = dr["task_name"] != DBNull.Value ? dr["task_name"].ToString() : "",
                            start_time = dr["start_time"] != DBNull.Value ? TimeSpan.Parse(dr["start_time"].ToString()) : default(TimeSpan),
                            stop_time = dr["stop_time"] != DBNull.Value ? TimeSpan.Parse(dr["stop_time"].ToString()) : default(TimeSpan),
                            note = dr["note"] != DBNull.Value ? dr["note"].ToString() : "",
                        };
                        whs.Add(wh);
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
            return whs;
        }

        public List<WorkingHoursModel> GetWorkingHours(string year, string month, string user)
        {
            List<WorkingHoursModel> whs = new List<WorkingHoursModel>();
            try
            {
                string string_command = string.Format($@"
                    SELECT
                        WorkingHours.ind,
                        WorkingHours.user_id,
                        Users.Name,
                        WorkingHours.working_date,
                        WorkingHours.week_number,
                        WorkingHours.job_id,
                        Jobs.job_name,
                        WorkingHours.task_id,
                        Tasks.task_name,
                        WorkingHours.start_time,
                        WorkingHours.stop_time,
                        WorkingHours.note
                    FROM WorkingHours
                        LEFT JOIN gps_sale_tracking.dbo.Sale_User Users ON WorkingHours.user_id = Users.Login 
                        LEFT JOIN Jobs ON WorkingHours.job_id = Jobs.job_id
                        LEFT JOIN Tasks ON WorkingHours.task_id = Tasks.task_id
                    WHERE WorkingHours.working_date like '{year}-{month}%' 
                    AND WorkingHours.user_id ='{user}'");
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
                        WorkingHoursModel wh = new WorkingHoursModel()
                        {
                            index = dr["ind"] != DBNull.Value ? Convert.ToInt32(dr["ind"]) : default(Int32),
                            user_id = dr["user_id"] != DBNull.Value ? dr["user_id"].ToString() : "",
                            user_name = dr["Name"] != DBNull.Value ? dr["Name"].ToString() : "",
                            working_date = dr["working_date"] != DBNull.Value ? Convert.ToDateTime(dr["working_date"]) : default(DateTime),
                            week_number = dr["week_number"] != DBNull.Value ? Convert.ToInt32(dr["week_number"]) : default(Int32),
                            job_id = dr["job_id"] != DBNull.Value ? dr["job_id"].ToString() : "",
                            job_name = dr["job_name"] != DBNull.Value ? dr["job_name"].ToString() : "",
                            task_id = dr["task_id"] != DBNull.Value ? dr["task_id"].ToString() : "",
                            task_name = dr["task_name"] != DBNull.Value ? dr["task_name"].ToString() : "",
                            start_time = dr["start_time"] != DBNull.Value ? TimeSpan.Parse(dr["start_time"].ToString()) : default(TimeSpan),
                            stop_time = dr["stop_time"] != DBNull.Value ? TimeSpan.Parse(dr["stop_time"].ToString()) : default(TimeSpan),
                            note = dr["note"] != DBNull.Value ? dr["note"].ToString() : "",
                        };
                        whs.Add(wh);
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
            return whs;
        }

        public List<WorkingHoursModel> GetWorkingHours(string user, DateTime working_date)
        {
            List<WorkingHoursModel> whs = new List<WorkingHoursModel>();
            try
            {
                string string_command = string.Format($@"
                    SELECT
                        ind,
                        WorkingHours.user_id,
                        Users.Name,
                        WorkingHours.working_date,
                        WorkingHours.week_number,
                        WorkingHours.job_id,
                        Jobs.job_name,
                        WorkingHours.task_id,
                        Tasks.task_name,
                        WorkingHours.start_time,
                        WorkingHours.stop_time,
                        WorkingHours.note
                    FROM WorkingHours
                        LEFT JOIN gps_sale_tracking.dbo.Sale_User Users ON WorkingHours.user_id = Users.Login 
                        LEFT JOIN Jobs ON WorkingHours.job_id = Jobs.job_id
                        LEFT JOIN Tasks ON WorkingHours.task_id = Tasks.task_id
                    WHERE WorkingHours.user_id = '{user}'
                    AND WorkingHours.working_date LIKE '{working_date.ToString("yyyy-MM-dd")}'");
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
                        WorkingHoursModel wh = new WorkingHoursModel()
                        {
                            index = dr["ind"] != DBNull.Value ? Convert.ToInt32(dr["ind"]) : default(Int32),
                            user_id = dr["user_id"] != DBNull.Value ? dr["user_id"].ToString() : "",
                            user_name = dr["Name"] != DBNull.Value ? dr["Name"].ToString() : "",
                            working_date = dr["working_date"] != DBNull.Value ? Convert.ToDateTime(dr["working_date"]) : default(DateTime),
                            week_number = dr["week_number"] != DBNull.Value ? Convert.ToInt32(dr["week_number"]) : default(Int32),
                            job_id = dr["job_id"] != DBNull.Value ? dr["job_id"].ToString() : "",
                            job_name = dr["job_name"] != DBNull.Value ? dr["job_name"].ToString() : "",
                            task_id = dr["task_id"] != DBNull.Value ? dr["task_id"].ToString() : "",
                            task_name = dr["task_name"] != DBNull.Value ? dr["task_name"].ToString() : "",
                            start_time = dr["start_time"] != DBNull.Value ? TimeSpan.Parse(dr["start_time"].ToString()) : default(TimeSpan),
                            stop_time = dr["stop_time"] != DBNull.Value ? TimeSpan.Parse(dr["stop_time"].ToString()) : default(TimeSpan),
                            note = dr["note"] != DBNull.Value ? dr["note"].ToString() : "",
                        };
                        whs.Add(wh);
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
            return whs;
        }

        public string AddWorkingHours(WorkingHoursModel wh)
        {
            try
            {
                string string_command = string.Format($@"
                    INSERT INTO WorkingHours(
                        user_id, working_date, week_number, job_id, task_id, start_time, stop_time, lunch, dinner, note)
                    VALUES (
                        @user_id, @working_date, (SELECT DATEPART(ISO_WEEK,@working_date)), @job_id, @task_id, @start_time, @stop_time, @lunch, @dinner, @note)");
                using (SqlCommand cmd = new SqlCommand(string_command, ConnectSQL.OpenConnect()))
                {
                    cmd.CommandType = System.Data.CommandType.Text;
                    cmd.Parameters.AddWithValue("@user_id", wh.user_id);
                    cmd.Parameters.AddWithValue("@working_date", wh.working_date);
                    cmd.Parameters.AddWithValue("@job_id", wh.job_id);
                    cmd.Parameters.AddWithValue("@task_id", wh.task_id);
                    cmd.Parameters.AddWithValue("@start_time", wh.start_time);
                    cmd.Parameters.AddWithValue("@stop_time", wh.stop_time);
                    cmd.Parameters.AddWithValue("@lunch", wh.lunch);
                    cmd.Parameters.AddWithValue("@dinner", wh.dinner);
                    cmd.Parameters.AddWithValue("@note", wh.note);
                    if (ConnectSQL.con.State != System.Data.ConnectionState.Open)
                    {
                        ConnectSQL.CloseConnect();
                        ConnectSQL.OpenConnect();
                    }
                    cmd.ExecuteNonQuery();
                }
            }
            catch(Exception ex)
            {
                return ex.Message;
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

        public string UpdateWorkingHours(WorkingHoursModel wh)
        {
            try
            {
                string string_command = string.Format($@"
                    UPDATE WorkingHours 
                    SET
                        user_id = @user_id,
                        working_date = @working_date,
                        week_number = (SELECT DATEPART(ISO_WEEK,@working_date)),
                        job_id = @job_id,
                        task_id = @task_id,
                        start_time = @start_time,
                        stop_time = @stop_time,
                        note = @note
                    WHERE ind = @ind");
                using (SqlCommand cmd = new SqlCommand(string_command,ConnectSQL.OpenConnect()))
                {
                    cmd.CommandType = System.Data.CommandType.Text;
                    cmd.Parameters.AddWithValue("@user_id", wh.user_id);
                    cmd.Parameters.AddWithValue("@working_date", wh.working_date);
                    cmd.Parameters.AddWithValue("@job_id", wh.job_id);
                    cmd.Parameters.AddWithValue("@task_id", wh.task_id);
                    cmd.Parameters.AddWithValue("@start_time", wh.start_time);
                    cmd.Parameters.AddWithValue("@stop_time", wh.stop_time);
                    cmd.Parameters.AddWithValue("@note", wh.note);
                    cmd.Parameters.AddWithValue("@ind", wh.index);
                    if (ConnectSQL.con.State != System.Data.ConnectionState.Open)
                    {
                        ConnectSQL.CloseConnect();
                        ConnectSQL.OpenConnect();
                    }
                    cmd.ExecuteNonQuery();
                }
            }
            catch (Exception ex)
            {
                return ex.Message;
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

        public string DeleteWorkingHours(WorkingHoursModel wh)
        {
            try
            {
                string string_command = string.Format($@"
                    DELETE FROM WorkingHours
                    WHERE user_id = @user_id
                        AND working_date = @working_date
                        AND job_id = @job_id
                        AND task_id = @task_id
                        AND start_time = @start_time
                        AND stop_time = @stop_time");
                using (SqlCommand cmd = new SqlCommand(string_command, ConnectSQL.OpenConnect()))
                {
                    cmd.CommandType = System.Data.CommandType.Text;
                    cmd.Parameters.AddWithValue("@user_id", wh.user_id);
                    cmd.Parameters.AddWithValue("@working_date", wh.working_date);
                    cmd.Parameters.AddWithValue("@job_id", wh.job_id);
                    cmd.Parameters.AddWithValue("@task_id", wh.task_id);
                    cmd.Parameters.AddWithValue("@start_time", wh.start_time);
                    cmd.Parameters.AddWithValue("@stop_time", wh.stop_time);
                    if (ConnectSQL.con.State != System.Data.ConnectionState.Open)
                    {
                        ConnectSQL.CloseConnect();
                        ConnectSQL.OpenConnect();
                    }
                    cmd.ExecuteNonQuery();
                }
            }
            catch (Exception ex)
            {
                return ex.Message;
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
